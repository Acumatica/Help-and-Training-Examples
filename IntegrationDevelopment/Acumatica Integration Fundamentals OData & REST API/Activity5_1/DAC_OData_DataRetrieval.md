# DAC-Based OData: Data Retrieval {#_27f3a57a-0def-48ad-ba80-3ab664a98fde .concept}

In this topic, you can find details about how to retrieve data by using the DAC-based OData interface. To retrieve the data, you append the DAC name and various parameters to the base URL of the DAC-based OData interface. For details about the base URL, see [The URL for the DAC-Based OData Interface](RPT_DAC_OData_GeneralInfo.md#_17597c9b-c7c4-4659-ae6a-f15108e59eee).

## Retrieving All Records of a DAC { .section}

You can use the following request to get all records of a DAC.

```
GET https://<Acumatica ERP instance URL>/t/<TenantName>/api/odata/dac/<DACName>
```

In this URL, the DAC is specified by its fully qualified name, by its display name, or by its short name \(which does not include the namespace\). For example, the use of either of the following URLs receives the same contents of the SOOrder DAC:

-   *https://sweetlife.com/erp/t/U100/api/odata/dac/SOOrder*
-   *https://sweetlife.com/erp/t/U100/api/odata/dac/PX\_Objects\_SO\_SOOrder*

**Tip:** Password fields are retrieved encrypted.

## Retrieving a Record by Its Key Fields { .section}

You can retrieve a record with certain values of key fields by specifying these fields and their values in the URL.

The following sample request retrieves the sales order for which the `OrderType` field is set to *SO* and the `OrderNbr` field is set to *000058*.

```
GET https://localhost/AcumaticaERP/t/U100/api/odata/dac/SOOrder(OrderType='SO',OrderNbr='000058')
```

## Specifying a Filter { .section}

You can obtain the records that meet a particular filter’s criteria by specifying this filter in the *$filter* parameter of the URL. You use [OData URL conventions](http://docs.oasis-open.org/odata/odata/v4.01/odata-v4.01-part2-url-conventions.html) to specify the conditions in the *$filter* parameter.

The following sample URL retrieves the sales orders whose `OrderType` field value is *IN*.

```
GET https://localhost/AcumaticaERP/t/U100/api/odata/dac/SOOrder?
$filter=(OrderType eq 'IN')
```

## Retrieving Related Data { .section}

You can obtain records along with related data, such as detail records or other linked records. For example, consider a sales order record: Each sales order record may have one or more detail records, which contain data about the inventory items included in the sales order. A sales order record also includes the identifier of the customer, which can be used to retrieve the linked customer record. Therefore, along with a sales order record, you can retrieve detail records and the linked customer record.

You can obtain the records, along with their detail records, in a single query by using the *$expand* parameter and the needed navigation property.

The following sample request retrieves sales orders along with their detail information.

```
GET https://localhost/AcumaticaERP/t/U100/api/odata/dac/SOOrder?
$filter=(OrderType eq 'IN')&
$expand=SOLineCollection
```

The following sample request retrieves sales orders along with data of the customer record related to each of those sales orders.

```
GET https://localhost/AcumaticaERP/t/U100/api/odata/dac/SOOrder?
$filter=(OrderType eq 'IN')&
$expand=BAccountByCustomerID
```

To retrieve records with multiple kinds of detail lines from Acumatica ERP by using DAC-based OData, in the *$expand* parameter, you use multiple navigation properties separated with comma.

## Specifying Particular Fields in a Response { .section}

In a request, you can specify the DAC fields that you want the response to contain. You do this by specifying the desired fields in the *$select* parameter.

The following sample request retrieves customers and provides the `AcctCD`, `AcctName`, and `CustomerClassID` fields in the response.

```
GET https://localhost/AcumaticaERP/t/U100/api/odata/dac/PX_Objects_AR_Customer?
$select=AcctCD,AcctName,CustomerClassID
```

To list the fields of the detail records, you specify the following in the request:

-   In the *$expand* parameter, the navigation property for the detail DAC
-   After the property, in parentheses, the DAC fields as the values of the *$select* parameter

**Tip:** By default, the maximum depth of expansion of the detail DACs in the *$expand* parameter is 3. You can change this value by specifying the value of the maxExpansionDepth attribute of the odata tag, which is available inside the px.core tag in the `Web.config` file of your Acumatica ERP instance.

The following sample URL retrieves customers along with their default addresses.

```
GET https://localhost/AcumaticaERP/t/U100/api/odata/dac/PX_Objects_AR_Customer?
$select=AcctCD,AcctName,CustomerClassID&
$expand=AddressByDefAddressID(
$select=AddressLine1,AddressLine2,City,State,PostalCode)
```

The response includes the customers' `AcctCD`, `AcctName`, and `CustomerClassID` fields and the following fields of the customers’ addresses: `AddressLine1`, `AddressLine2`, `City`, `State`, and `PostalCode`.

## Retrieving Records in Batches { .section}

To retrieve records in batches from Acumatica ERP by using DAC-based OData, you need to use the *$top* and *$skip* parameters of the request. We recommend that you also use the *$orderby* parameter to specify the order of records.

**Attention:** If the *$orderby* parameter is not specified for retrieving batches of records, the system sorts the records by the key fields in ascending order.

The following example retrieves a batch of warehouse detail records. It indicates that the records should be ordered by inventory ID in ascending order. The system will retrieve the top 500 records, excluding the first 50.

```
GET https://localhost/AcumaticaERP/t/U100/api/odata/dac/PX_Objects_IN_INSiteStatus?
$skip=50&
$top=500&
$orderby=InventoryID
```

## Retrieving Multilingual Fields { .section}

If a record has multilingual fields, you can obtain the values of these fields in any available language. You can do this in either of the following ways:

-   By using the `Accept-Language` HTTP header and specifying the desired locale as its value
-   By using the `locale` URL parameter and specifying the desired locale as its value

If neither the `Accept-Language` HTTP header nor the `locale` URL parameter is used, the values of multilingual fields are returned in the default language \(see [Setting Up Languages](SM__CON_Locales_and_Languages.md#section_vkn_2lm_hw)\). If both the `Accept-Language` HTTP header and the `locale` URL parameter are used, the locale specified in the `locale` URL parameter is taken into account.

The following sample URL retrieves the *AACOMPUT01* inventory item with the French values of the multilingual fields.

```
GET https://localhost/AcumaticaERP/t/U100/api/odata/dac/PX_Objects_IN_InventoryItem?
locale=fr-FR&
$filter=InventoryCD eq 'AACOMPUT01'
```

The use of the following request has the same effect.

```
GET /AcumaticaERP/t/U100/api/odata/dac/PX_Objects_IN_InventoryItem?
$filter=InventoryCD eq 'AACOMPUT01' HTTP/1.1
Host: localhost
Accept-Language: fr-FR
```

## Retrieving Archived Records { .section}

You can retrieve archived records along with non-archived ones. To make archived records visible for requests, you use the `PX-ApiArchive` HTTP header with the *SHOW* value.

The following sample request retrieves the shipments \(including archived ones\) that were created before December 31, 2022.

```
GET /AcumaticaERP/t/U100/api/odata/dac/PX_Objects_SO_SOShipment?
$filter=ShipDate gt 2022-12-31T00:00:00%2b03:00
&$select=ShipmentNbr,ShipDate HTTP/1.1
Host: localhost
PX-ApiArchive: SHOW
```

## Retrieving Removed Records { .section}

The removed records are stored in the database if the corresponding database table has the DeletedDatabaseRecord column. For details about this mechanism, see [Preservation of Deleted Records \(DeletedDatabaseRecord\)](../StudioDeveloperGuide/DA__con_Preservation_of_Deleted_Records.md). By default, the removed records are not returned if you request data through the DAC-based OData interface.

To retrieve the removed records, you need to specify the `PX-ApiDeleted` HTTP header with the *SHOW* value in the request.

For example, the following request retrieves both removed customer records and customer records that have not been removed.

```
GET /AcumaticaERP/t/U100/api/OData/DAC/Customer?
$select=BAccountID,DeletedDatabaseRecord HTTP/1.1
Host: localhost
PX-ApiDeleted: SHOW
```

## Tracking Removed Records for Synchronization {#_264ee4ec-929f-4356-b06d-f1485147899a .section}

The optimal way to synchronize data between Acumatica ERP and an external system is to synchronize only the data that has been modified since the previous synchronization. To track the modification date, you can use the values in the LastModifiedDateTime field of a record. However, this type of synchronization does not take into account the records that have been removed since the last synchronization. To synchronize the removed records, you can use the system mechanism that tracks the removal of records of particular data access classes \(DACs\).

You can use this mechanism as follows:

1.  On the [Tables to Track Deleted Records](SM_20_70_10.md) \(SM207010\) form, you add the DACs for which the system should track the removed records. If you do not list a DAC, the system does not track its removed records.

    **Tip:** If a record of a DAC listed on the form is removed, the system saves the NoteID of the record and the date and time of its removal. The system does not keep the record itself.

2.  To obtain the list of removed records of a particular DAC, you use the px.GetDeletedRecords\(\) function in the request URL.

    You can also filter the records by the date of removal by using the DeleteDate field in the *$filter* parameter.

    **Attention:** A user can obtain the list of removed records of a DAC by using an OData request if the user has access to this DAC through DAC-based OData.

3.  Optional: To include this list of DACs in a customization project, you use the [Tables to Track Deleted Records](AU_20_60_05.md) page of the Customization Project Editor. You need to list the DACs on the [Tables to Track Deleted Records](SM_20_70_10.md) \(SM207010\) form first in order they are available for selection on the page.

For example, suppose that you need to retrieve the list of sales orders that have been removed since November 1, 2024, from the *U100* tenant of the local Acumatica ERP instance. Further suppose that the SOOrder DAC has been added to the list on the [Tables to Track Deleted Records](SM_20_70_10.md) \(SM207010\) form. You can execute the following request to obtain the list of removed records.

```
GET https://localhost/AcumaticaERP/t/U100/api/odata/dac/SOOrder/px.GetDeletedRecords()?$filter=DeleteDate ge 2024-11-01T00:00:00Z
```

## Retrieving Custom and User-Defined Fields { .section}

In a customization project, you can add custom fields to Acumatica ERP forms. You can also add user-defined fields to Acumatica ERP forms and include them in a customization project. \(For details about user-defined fields, see [User-Defined Fields](CS__con_User_Defined_Fields.md#).\)

To retrieve custom and user-defined fields through the DAC-based OData interface, you do not need to perform special actions. You address custom and user-defined fields by their names.


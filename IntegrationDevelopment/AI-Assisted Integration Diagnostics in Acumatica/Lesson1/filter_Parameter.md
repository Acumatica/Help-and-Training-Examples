# $filter Parameter

When you retrieve records from Acumatica ERP by using the contract-based REST API, you use the `$filter` parameter to specify the conditions that determine which records should be selected from Acumatica ERP. You use [OData URI conventions](http://www.odata.org/documentation/odata-version-3-0/url-conventions/) to specify the value of the parameter.

You can specify multiple conditions for the same field or different fields in a filter by using the AND and OR operators.

When you specify the value of the parameter, you can use the following functions as they are defined in OData:

- `substringof`
- `startswith`
- `endswith`

You can use the following custom function to filter records by the values of custom fields: `cf.<Type name>(f='<View name>.<Field name>')`, where `<Type name>` is the type of the custom element, `<View name>` is the name of the data view that contains the element, and `<Field name>` is the name of the element.

## Example: Simple Condition

To obtain stock item records that have the **Active** status in Acumatica ERP, you use the following filter: `$filter=ItemStatus eq 'Active'`.

## Example: Condition on a Linked Entity

To obtain a customer record that has the **demo@gmail.com** email address, you use the following filter: `$filter=MainContact/Email eq 'demo@gmail.com'`. The `Email` field is defined in a linked entity, which is available through the `MainContact` property.

> Attention: The REST API does not support filtering on detail records. If you specify a filter on detail records, the results of the request cannot be predicted.

## Example: Multiple Conditions

To obtain stock item records that have the **Active** status in Acumatica ERP and have been modified later than July 15, 2024, you use the following filter: `$filter=ItemStatus eq 'Active' and LastModified gt datetimeoffset'2024-07-15T10%3A31%3A28.402%2B03%3A00'`.

> Attention: You should encode date and time values in URL format before passing them as the value of the parameter. For example, you can encode the current date and time by using the `System.Net.WebUtility.URLEncode()` method as follows: `WebUtility.UrlEncode(new DateTimeOffset(DateTime.Now).ToString("yyyy-MM-ddTHH:mm:ss.fffK"))`.

## Example: Condition with a Date-Only Field

If you want to filter records by a date-only field, you use the `date` function in the `$filter` parameter. For example, if you need to obtain all records with the `Date` value greater than June 17, 2025, you use the following filter: `$filter=Date gt date'2025-06-17'`.

## Example: Condition on a Field Not Defined in the Endpoint

Suppose that in an extension of the **Default/25.200.001** endpoint, you added the `UsrRepairItemType` field to the top-level `StockItem` entity. This field corresponds to the **Repair Item Type** custom element, which has been added to the **General** tab, in the **Item Defaults** section, which corresponds to the `ItemSettings` data view of the Stock Items (IN202500) form. If you want to obtain all records on the Stock Items form for which the value of the custom **Repair Item Type** element is **Battery**, you would use the following parameter string: `$filter=cf.String(f='ItemSettings.UsrRepairItemType') eq 'Battery'`.
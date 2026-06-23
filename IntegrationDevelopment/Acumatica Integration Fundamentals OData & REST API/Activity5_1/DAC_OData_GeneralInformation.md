# DAC-Based OData: General Information {#_96d2641e-580a-4ea1-aad9-238235c449de .concept}

In your browser or in another application that works with OData, you can view the data that is available in your Acumatica ERP instance through the OData interface that retrieves data from data access classes \(DACs\)—that is, the DAC-based OData interface. Acumatica ERP supports the [OData Version 4.0](https://www.odata.org/documentation/) protocol for DAC-based OData.

## Learning Objectives { .section}

In this chapter, you will learn how to do the following:

In this lesson, you will learn how to do the following:

-   Sign in to Acumatica ERP to use the OData interface
-   Access data that is exposed through the DAC-based OData interface
-   Configure Cross-Origin Resource Sharing \(CORS\) to access data through client-side web applications

## Applicable Scenarios { .section}

You may find the information in this chapter useful when you are a technical specialist with your company and your responsibilities include the management of reports and inquiries.You may find the information in this lesson useful when you are a technical specialist with your company and your responsibilities include the management of reports and inquiries. An accountant or other employee may have requested access to Acumatica ERP data through a third-party OData client, such as Microsoft Excel, Microsoft Power BI, or a Java-based application.

You may also find this information useful if you are a developer who creates an integration application that needs to retrieve data from Acumatica ERP.

## Access to DAC Records Through an OData Client { .section}

You can expose data from Acumatica ERP—in this case, records from DACs—through the OData interface. With this data, users can perform detailed financial analysis by using third-party OData clients, such as Microsoft Excel and Microsoft Power BI.

The DAC-based OData interface uses the basic authentication in Acumatica ERP. That is, you need to sign in with a username and password to Acumatica ERP before you request the data through the OData protocol. To connect to your data, you specify the URL of the DAC-based OData endpoint of your Acumatica ERP instance to the OData client and authenticate yourself by entering your Acumatica ERP credentials. The OData client then connects to your Acumatica ERP instance and obtains the data for you.

To view the data in the browser, you enter the URL of the DAC-based OData interface of your Acumatica ERP instance in the address bar of your browser. When the system asks you to authenticate yourself, you provide your Acumatica ERP username and password. When your identity has been confirmed, the system displays the requested data.

**Tip:** When the system calculates the number of signed-in users, it treats the basic authentication used for OData as a sign-in of a conventional user \(not an API user\). The license restriction for conventional users is shown in the **Concurrent Users** box on the **License** tab of the [License Monitoring Console](../Shared/../UserGuide/SM_60_40_00.md) \(SM604000\) form.

The OData client can use the OAuth 2.0 authorization instead of direct authentication with a username and password. For details about OAuth 2.0 authorization, see [Authorizing Client Applications to Work with Acumatica ERP](../dev_Area_Untitled1.md#).

## The URL for the DAC-Based OData Interface {#_17597c9b-c7c4-4659-ae6a-f15108e59eee .section}

The URL for the DAC-based OData interface is *&lt;Acumatica ERP instance URL&gt;/t/&lt;TenantName&gt;/api/odata/dac*. In this URL, *&lt;TenantName&gt;* is the login name of the tenant in the Acumatica ERP instance. \(For more information on single-tenant and multitenant configuration, see [Tenants: General Information](SA_Managing_Tenants_Using_Web_GeneralInfo.md).\) A request to this URL returns the list of all DACs in the tenant.

**Attention:** Through the DAC-based OData interface, users have access to the same data that is visible to them in the UI based on their access rights.

For example, you would specify the *https://sweetlife.com/erp/t/Calipso LLC/api/odata/dac* URL if the following are true:

-   The URL of the Acumatica ERP instance is *https://sweetlife.com/erp*.
-   The instance contains the *Calipso LLC* tenant.
-   You want to obtain the list of DACs in this tenant.

If you type this sample URL into a browser, you will notice that the browser automatically replaces the space with *%20*.

**Tip:** You can find the login names of tenants in the **Login Name** column on the [Tenant List](../Shared/../UserGuide/SM_20_35_30.md) \(SM203530\) form, as shown in the following screenshot.

![](../Shared/Images/GI_Access_to_Exposed_Inquiry_Through_Excel_GI_OData_TenantLoginName.png "The login names of tenants")

Also, you can view the login name of the tenant to which you are currently signed in by viewing the User menu \(as shown in the following screenshot\), which you access by clicking the User menu button on the top pane of the Acumatica ERP screen.

![](../Shared/Images/GI_Access_to_Exposed_Inquiry_Through_Excel_GI_OData_TenantLoginName_InfoArea.png "The User menu with the tenant login name")

The following code fragment shows a list of DACs; the list has been retrieved through the OData interface.

```json
{
  "@odata.context": 
    "<Acumatica ERP instance URL>/t/<TenantName>/api/odata/dac/$metadata",
  "value": [
    {
      "name": "PX_Api_Mobile_Workspaces_MobileSiteMapWorkspaces",
      "kind": "EntitySet",
      "url": "PX_Api_Mobile_Workspaces_MobileSiteMapWorkspaces"
    },
    {
      "name": "PX_Api_SYMapping",
      "kind": "EntitySet",
      "url": "PX_Api_SYMapping"
    },
    {
      "name": "Mapping",
      "kind": "EntitySet",
      "url": "Mapping"
    },
    ...
  ]
}
```

## Retrieval of a Tenant’s Full Set of Metadata { .section}

You can use the following request to get the metadata of a tenant of an Acumatica ERP instance.

```
GET https://<Acumatica ERP instance URL>/t/<TenantName>/api/odata/dac/$metadata 
```

The metadata includes the following data:

-   The full list of DACs. For each DAC, the `EntityType` tag contains the name of the DAC.
-   The fields of the DACs and the fields' types. You can find the list of fields of the DAC in the `Property` tags nested in the `EntityType` tag.
-   The related DACs, which are defined by the `NavigationProperty` tags nested in the `EntityType` tag.

    The navigation properties whose name is in the `<detail_entity_type>Collection` format refer to detail entities. The navigation properties with the name in the `<related_entity_type>By<entity_field>` format refer to related entities.

    For example, the definition of the `PX.Objects.SO.SOOrder` DAC has the `SOLineCollection` navigation property, which states that the `PX.Objects.SO.SOLine` DAC is a detail entity. The definition of the `PX.Objects.SO.SOOrder` DAC also has the `BAccountByCustomerID` navigation property, which links the `CustomerID` field of the `PX.Objects.SO.SOOrder` DAC with the `BAccountID` field of the `PX.Objects.CR.BAccount` DAC.


You use the DAC names and fields from the metadata to retrieve the data from Acumatica ERP, as described in [DAC-Based OData: Data Retrieval](RPT_DAC_OData_DataRetrieval.md).

## Support of the OData Specification { .section}

The OData protocol provided by Acumatica ERP for the retrieval of data access classes \(DACs\) does not support aggregation functions. These functions are defined in the OData Version 4.0 standard extension \(see [OData Extension for Data Aggregation Version 4.0](http://docs.oasis-open.org/odata/odata-data-aggregation-ext/v4.0/odata-data-aggregation-ext-v4.0.html)\).

## Configuration of CORS { .section}

Acumatica ERP supports Cross-Origin Resource Sharing \(CORS\), meaning that requests for resources can come from a different domain than that of the resource making the request. With CORS enabled, you can allow access to the OData endpoints of your Acumatica ERP instance for client-side web applications, including Java-based applications. For more information about CORS, see [Cross-Origin Resource Sharing](http://www.w3.org/TR/cors/) on the World Wide Web Consortium portal.

The CORS settings of the web server of your instance are defined by the `cors` section of the `Web.config` file; see the following example of the default configuration for this section.

```xml
<cors enabled="true" origins="*" methods="*" headers="*" 
exposedHeaders="DataServiceVersion,MaxDataServiceVersion,OData-Version,
OData-MaxVersion" />
```

By default, CORS is enabled, all origins are allowed access to the server, and all supported headers are exposed and available for use. The web server of the application supports simple headers as well as the following headers: `DataServiceVersion`, `MaxDataServiceVersion`, `OData-Version`, and `OData-MaxVersion`. You need to use these four headers to access OData endpoints.

You can enforce limitations on cross-origin requests by changing the settings in the `Web.config` file. You can add your own headers as well. For details, see [Generic Inquiry Access Through OData: To Configure CORS](../Shared/../UserGuide/CU__how_Odata_CORS.md).


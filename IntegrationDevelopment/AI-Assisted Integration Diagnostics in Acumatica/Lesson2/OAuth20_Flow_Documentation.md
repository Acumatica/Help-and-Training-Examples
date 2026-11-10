# Authorizing Client Applications to Work with Acumatica ERP {#_a8f71c44-9f5c-4af8-9d47-bc815c8a58e7 .concept}

Acumatica ERP supports the OAuth 2.0 mechanism of authorization and OpenID Connect \(OIDC\) authentication protocol for applications that are integrated with Acumatica ERP through web services application programming interfaces \(APIs\) or OData. When a client application of Acumatica ERP uses OAuth 2.0 or OIDC, the client application does not operate with the Acumatica ERP credentials to sign a user in to Acumatica ERP; instead, the application obtains an access token from Acumatica ERP and uses this token when it requests data from Acumatica ERP.

Depending on the flow that the client application implements, the client application either has no information on the credentials of an Acumatica ERP user or uses this information only once to obtain the access token. OAuth 2.0 or OIDC improves the security of the Acumatica ERP data accessed by the application and simplifies the management of access rights.

The client application that implements OAuth 2.0 or OIDC can use one of the authorization flows supported by Acumatica ERP, which are the following:

-   Authorization Code \(OAuth 2.0 and OIDC\)
-   Implicit \(OAuth 2.0 and OIDC\)
-   Resource Owner Password Credentials \(OAuth 2.0\)
-   Hybrid \(OIDC\)

In this part, you can find details on the authorization flows and information about how to register the OAuth 2.0 or OIDC client applications and revoke access of the applications.

## Getting Started with OAuth 2.0 and OpenID Connect Authorization {#_a5a25d1d-e345-4154-a398-3bff4898836c}

OAuth 2.0 and OpenID Connect \(OIDC\) are the protocols that can be used for authentication and authorization in Acumatica ERP.

OAuth 2.0 enables third-party applications to obtain limited access to Acumatica ERP web services on behalf of a resource owner. It can be used for enabling secure access to the web services without sharing user credentials. OAuth 2.0 uses access tokens to grant access to resources.

OIDC extends OAuth 2.0 by adding an identity layer on top of the authorization process. It allows client applications to verify the identity of users based on the authentication performed by an authorization server, as well as to obtain basic profile information about the authenticated user. OIDC introduces the concept of an ID token, which is a JSON Web Token \(JWT\) that contains identity information about the user.

In this chapter, you can find overview information about support of OAuth 2.0 and OIDC in Acumatica ERP and learn about general steps you need to perform to implement OAuth 2.0 or OIDC in your application. The chapter also contains details about the implementation of the common steps for each authorization flow.

### OAuth 2.0 and OIDC: General Information {#_c6294b6b-a460-491e-8130-ae55d063c15c}

OAuth 2.0 and OpenID Connect \(OIDC\) are used in scenarios where secure authentication and authorization are required. The implementation of these mechanisms includes multiple steps that you need to do in the client application and in Acumatica ERP.

#### Learning Objectives { .section}

In this chapter, you will learn the following:

-   Which steps you need to perform to implement OAuth 2.0 or OIDC
-   What the differences between the flows are
-   How to work with data in Acumatica ERP after successful authorization
-   How to refresh access to Acumatica ERP after access has expired

#### Applicable Scenarios { .section}

You implement OAuth 2.0 or OIDC authorization of a client application in the following scenarios:

-   You need to provide secure access to Acumatica ERP through the REST API, SOAP API, or OData without sharing user credentials.
-   You need to implement single sign-on solutions where users can sign in once and access multiple applications without having to sign in separately to each one.
-   Only for OIDC: In the client application, you need to verify the identity of users and obtain basic profile information from Acumatica ERP.

#### Authorization Implementation { .section}

To use OAuth 2.0 or OIDC, you need to perform the following general steps:

1.  You register the client application in Acumatica ERP.
2.  You implement the authorization flow in the client application.
3.  Optional: You implement the refreshing of the application access in the client application.
4.  You include the information about the connected application in the customization project.

**Attention:**

According to the OAuth 2.0 and OIDC specifications, a secure connection between a client application and the Acumatica ERP website with a Secure Sockets Layer \(SSL\) certificate is required. Therefore, you have to set up the Acumatica ERP website for HTTPS before the client application can work with data in Acumatica ERP. For more information, see [Preparation for the Acumatica ERP Installation: System Environment](UserGuide/INST_Preparing_Installation_System_Environment.md).

#### Registration of the Application { .section}

Before an OAuth 2.0 or OIDC client application can work with Acumatica ERP, you must register this application in Acumatica ERP. For details about registration, see [Registration of an OAuth 2.0 or OIDC Application: General Information](#_84e7f6c6-3b0f-4526-a1aa-343e39aeebfe).You will learn more details about registration in [Lesson 1.1: Registering the Application in Acumatica ERP](Trainings/I320/OAuth_Register.md).

#### Implementation of the Authorization Flow in the Client Application { .section}

An authorization flow is a sequence of steps that the client application and Acumatica ERP follow during the authorization process. The client application that implements OAuth 2.0 or OIDC can use one of the authorization flows supported by Acumatica ERP, which are the following:

-   Authorization Code \(OAuth 2.0 and OIDC\), which is described further in [Authorization Code Flow: General Information](#_ff780860-09c2-46c9-bdd7-c6c3b1fc442c)
-   Implicit \(OAuth 2.0 and OIDC\); for more information, see [Implicit Flow: General Information](#_76861c67-265c-46f4-949d-c8d4509c99ec)
-   Resource Owner Password Credentials \(OAuth 2.0\), as described in [Resource Owner Password Credentials Flow: General Information](#_2930d2f7-e081-4d0e-8879-93907ce82607)
-   Resource Owner Password Credentials \(OAuth 2.0\), which will be described in [Lesson 1.2: Configuring the Application to Use OAuth 2.0](Trainings/I320/OAuth_ConfigureApp.md)
-   Hybrid \(OIDC\), which is explored more fully in [Hybrid Flow: General Information](#_f1bcf512-d676-4d8d-99d7-1a25fd565cdf)

Each authorization flow has its own use cases and security considerations, as you can see in [OAuth 2.0 and OIDC: Comparison of the Flows](#_f7add280-bc3c-4e73-b242-580c94af10a2). The choice of the flow depends on multiple factors, such as the type of client application, the level of trust between the client and the authorization server, and the security requirements of the application.

#### Refreshing of the Application Access { .section}

The access token, which the client application obtains from Acumatica ERP during authorization of the application, is valid for a specific period of time, which is specified in the response that returns the access token. When the access token expires, the client application can request a new access token by providing the refresh token to the token endpoint. For details about refreshing the application access, see [OAuth 2.0 and OIDC: Refreshing of an Access Token](#_2a5781c6-b661-409c-82f3-8adafd6a332d).

#### Inclusion of a Connected Application in a Customization Project { .section}

If you need to use a client application that implements the OAuth 2.0 or OpenID Connect authorization mechanism with other Acumatica ERP instances, you need to include the information about this client application in a customization project and publish this customization project to these instances. To include the information about the registered client application in a customization project, you use the [Connected Applications](UserGuide/AU_21_00_30.md) page of the Customization Project Editor.

#### Revocation of the Application Access {#_d0ae7bf3-ee6d-4dd5-9cf2-37cfedeceb9a .section}

To revoke the access of an OAuth 2.0 or OpenID Connect client application, you can use either of the following Acumatica ERP forms:

-   [Connected Applications](UserGuide/SM_30_30_10.md) \(SM303010\): On this form, you can revoke the access of any application registered in the current company. You revoke all access granted to the application.
-   [User Profile](UserGuide/SM_20_30_10.md) \(SM203010\): On this form, you can revoke the access of any application to which you \(that is, the user account to which you are signed in\) have granted access. Any access granted to this application by other users remains unchanged.

After you have revoked access, the related access tokens are removed from the Acumatica ERP database, and these tokens cannot be used to access data in Acumatica ERP. However, the client secrets remain valid until their expiration dates \(if applicable\), and the application can use these secrets to request a new access token.

### OAuth 2.0 and OIDC: Comparison of the Flows {#_f7add280-bc3c-4e73-b242-580c94af10a2}

The table below summarizes the characteristics of the authorization flows supported by Acumatica ERP.

|Characteristic|Authorization Code flow|Implicit flow|Resource Owner Password Credentials flow|Hybrid flow|
|--------------|-----------------------|-------------|----------------------------------------|-----------|
|The OAuth 2.0 authorization mechanism is available.|Yes|Yes|Yes|No|
|OpenID Connect \(OIDC\) is available.|Yes|Yes|No|Yes|
|The access token is returned from the authorization endpoint.|No|Yes|No|Yes|
|The access token is returned from the token endpoint.|Yes|No|Yes|Yes|
|The refresh token can be issued.|Yes|No|Yes|Yes|
|The client application has access to Acumatica ERP credentials \(username and password\).|No|No|Yes|No|
|The user explicitly grants access to the requested scopes.|Yes|Yes|No|Yes|
|The client application is authenticated in Acumatica ERP \(that is, the client application provides the client ID and client secret or the client ID and JWT bearer token\).|Yes|No|Yes|Yes|

**Related information**  


[Authorization Code Flow: General Information](#_ff780860-09c2-46c9-bdd7-c6c3b1fc442c)

[Implicit Flow: General Information](#_76861c67-265c-46f4-949d-c8d4509c99ec)

[Resource Owner Password Credentials Flow: General Information](#_2930d2f7-e081-4d0e-8879-93907ce82607)

[Hybrid Flow: General Information](#_f1bcf512-d676-4d8d-99d7-1a25fd565cdf)

### OAuth 2.0 and OIDC: Working with Data in Acumatica ERP {#_ed9fe5ab-b3c0-444c-8335-ff3b0c8392d6}

To obtain the data from Acumatica ERP or submit data to the system, the client application connects to the web services API or OData endpoint of Acumatica ERP with the needed HTTP method. The following sections provide details on the request.

#### HTTP Method and URL { .section}

A client application can use the REST API, screen-based SOAP API, or OData if a user has granted access to them for the client application. For details on the methods and URLs that can be used to retrieve or submit data, see one of the following topics:

-   [Configuring the REST API](IntegrationDevelopmentGuide/IS__mng_Contract_Based_Web_Services.md)
-   [Accessing the Exposed Inquiry Results Through OData](UserGuide/GI_Access_to_Exposed_Inquiry_Through_OData_Mapref.md)
-   [Accessing DACs Through OData](UserGuide/RPT_DAC_OData_Mapref.md)
-   [Working with the SOAP API](IntegrationDevelopmentGuide/IS__mng_SOAP_API.md)

#### HTTP Header { .section}

When you are working with Acumatica ERP data, you use the following HTTP header.

|Key|Value|
|---|-----|
|`Authorization`|The token type, which is *Bearer*, and the access token that the client application has received from the authorization or token endpoint. The client application should include the access token in the `Authorization` header of each request to Acumatica ERP.|

#### Example { .section}

The following example retrieves a sales order from the *Default/25.200.001* endpoint through the REST API. The access token is used for authorization.

```
GET /AcumaticaDB/entity/Default/25.200.001/SalesOrder/SO/000001 HTTP/1.1
Host: localhost
Authorization: Bearer cde78a99a2dc6388eb8c7242a90cf9bc
```

### OAuth 2.0 and OIDC: Refreshing of an Access Token {#_2a5781c6-b661-409c-82f3-8adafd6a332d}

An access token is valid for a specific period of time, which is specified in the response that returns the access token. When the access token expires, the client application can request a new access token by providing the refresh token to the token endpoint. To request a new access token, the client application should use the `POST` method. The following sections provide details on the request and the response.

#### HTTP Method and URL { .section}

To request a new access token, the client application should use the `POST` HTTP method. The client application can use one of the following approaches for the URL:

-   If the client application supports OpenID Connect Discovery, the client application can use the discovery endpoint address, as shown in the following code.

    ```
    https://<Acumatica ERP instance URL>/identity/.well-known/openid-configuration
    ```

    **Tip:** We recommend that the client application use the discovery endpoint address to obtain the token endpoint address. The use of the discovery endpoint eliminates the need to change the application if the address of the token endpoint changes.

    **Attention:** A request to the discovery endpoint does not provide the access token; it provides the address of the token endpoint from which you can receive the access token.

-   The client application can directly use the token endpoint address, as shown in the following code.

    ```
    https://<Acumatica ERP instance URL>/identity/connect/token
    ```


#### HTTP Header { .section}

To refresh an access token, you use the following HTTP header.

|Key|Value|
|---|-----|
|`Content-Type`|`application/x-www-form-urlencoded`|

#### Request Body { .section}

To refresh an access token, you specify the following parameters in the request body.

<table id="_078eeb23-0d10-4761-8c7c-31c017f10dd1"><thead><tr><th>

Parameter

</th><th>

Description

</th></tr></thead><tbody><tr><td>

grant\_type

</td><td>

The type of the request, which must be set to `refresh_token` for the request of the refresh token.

</td></tr><tr><td>

client\_id

</td><td>

The client ID that was assigned to the client application during the registration of the application in Acumatica ERP. The client ID must have the format in which the ID was generated during the registration of the application. That is, the client ID must include an auto-generated string and the ID of the tenant, such as *88358B02-A48D-A50E-F710-39C1636C30F6@MyTenant*. The client application will have access to the data of the tenant specified in the client ID.

</td></tr><tr><td>

client\_secret

</td><td>

For a client application that uses a shared secret, the value of the secret that was created for the client application during the registration of the application in Acumatica ERP.

</td></tr><tr><td>

client\_assertion\_type

</td><td>

For a client application that uses JSON Web Token \(JWT\) bearer tokens, the client assertion type, which must be set to *urn:ietf:params:oauth:client-assertion-type:jwt-bearer*.

</td></tr><tr><td>

client\_assertion

</td><td>

For a client application that uses JSON Web Token \(JWT\) bearer tokens, a single JWT.

</td></tr><tr><td>

refresh\_token

</td><td>

The refresh token that the client application received from the token endpoint along with the access token if a user granted the offline\_access scope to the client application.

</td></tr></tbody>
</table>#### Response Body {#_c3ae6faf-4898-4097-a3f3-2e74200d4939 .section}

Acumatica ERP verifies the provided application credentials and issues the new access token and the new refresh token. To request the access token once again, the client application should use the latest issued refresh token. That is, if the client application has received a new refresh token, the client application should discard the previous refresh token and use the new one.

A successful response includes the following parameters in the response body.

<table id="_dc2096a3-af1b-49f8-a580-f26674cc082d"><thead><tr><th>

Parameter

</th><th>

Description

</th></tr></thead><tbody><tr><td>

token\_type

</td><td>

The type of the access token, which is *Bearer*.

</td></tr><tr><td>

access\_token

</td><td>

The new access token.

</td></tr><tr><td>

expires\_in

</td><td>

The period of time \(in seconds\) during which the access token is valid.

</td></tr><tr><td>

scope

</td><td>

The scope for which the access token and ID token are provided. The returning of this parameter is optional.

</td></tr><tr><td>

refresh\_token

</td><td>

The new refresh token.

</td></tr><tr><td>

id\_token

</td><td>

The ID token associated with the authenticated session. The ID token contains three parts, which are separated by periods. The parts are Base64 encoded. The second part contains the claims to which the user granted access. For details on the ID token structure, see [https://openid.net/specs/openid-connect-core-1\_0.html\#IDToken](https://openid.net/specs/openid-connect-core-1_0.html#IDToken) and [https://www.rfc-editor.org/rfc/rfc7519.html](https://www.rfc-editor.org/rfc/rfc7519.html). We recommend that you use the existing standard libraries for parsing the tokens. The parameter is returned only if the openid scope was granted.

</td></tr></tbody>
</table>### OAuth 2.0 and OIDC: Obtaining of the User Data {#_065d2515-3e48-4ac6-8c62-56ae546f92eb}

To obtain the user data, the client application can connect to the user information endpoint of Acumatica ERP with the `GET` HTTP method. See details on the request and the response in the following sections.

**Attention:** The way of obtaining user data that is described in this topic is optional. The recommended way is to parse the validated ID token, which contains the same claims as the ones that are obtained through the request described in this topic. The recommended way does not require an additional call to Acumatica ERP.

#### HTTP Method and URL { .section}

The client application connects to the user information endpoint of Acumatica ERP with the `GET` HTTP method. The client application can use one of the following options for the URL:

-   If the client application supports OpenID Connect Discovery, the client application can use the discovery endpoint address, as shown in the following code.

    ```
    https://<Acumatica ERP instance URL>/identity/.well-known/openid-configuration
    ```

    **Tip:** We recommend that the client application use the discovery endpoint address to obtain the token endpoint address. The use of the discovery endpoint eliminates the need to change the application if the address of the token endpoint changes.

    **Attention:** A request to the discovery endpoint does not provide the access token; it provides the address of the token endpoint from which you can receive the access token.

-   The client application can directly use the address of the user information endpoint, which is shown below.

    ```
    https://<Acumatica ERP instance URL>/identity/connect/userinfo
    ```


#### HTTP Header { .section}

To obtain the user data, you use the following HTTP header.

<table id="_eeecbe70-7315-4e1c-ad9d-f085dca6c9e3"><thead><tr><th>

Key

</th><th>

Value

</th></tr></thead><tbody><tr><td>

`Authorization`

</td><td>

The token type, which is *Bearer*, and the access token that the client application has received from the authorization or token endpoint. The client application should include the access token in the `Authorization` header of each request to Acumatica ERP. **Note:** For the application to obtain the user data, the access token must include the openid scope.

</td></tr></tbody>
</table>#### Response Body { .section}

The response body includes the claims to which the user has provided access in JSON format.

#### Example { .section}

An example of a request to the user information endpoint is shown below.

```
GET /AcumaticaDB/identity/connect/userinfo HTTP/1.1
Host: localhost
Authorization: Bearer cde78a99a2dc6388eb8c7242a90cf9bc
```

Acumatica ERP verifies the provided access token and returns the following data in the response body.

```
{
    "name": "Kimberly Gibbs",
    "given_name": "Kimberly",
    "family_name": "Gibbs",
    "preferred_username": "gibbs",
    "email": "gibbs@sweetlife.com",
    "zoneinfo": "",
    "updated_at": "1/1/1900 12:00:00 AM",
    "sub": "gibbs@U100"
}
```

### OAuth 2.0 and OIDC: Session Management {#_1a864bb9-3e45-4e94-82e1-a4d52331c3d6}

If you authorize your integration application to work with Acumatica ERP through OAuth 2.0, the sign-out is not always required after you have finished your work with Acumatica ERP. \(This is opposed to the situation when your integration application uses the API methods for the sign-in in Acumatica ERP—that is, uses cookies to manage the application sessions. For these applications, the sign-out is required to close the session each time the work with Acumatica ERP is finished.\)

#### Requirements for the Sign Out { .section}

Whether or not the sign-out is required depends on the access scope that the user has granted to the application as follows:

-   If the user have granted only the api scope to the application, the access token of the application expires in one hour and the session that was opened for this access token is closed automatically. However, if the application has been granted only the api scope, we recommend that you call the sign-out method after you have finished your work with Acumatica ERP, because the Acumatica ERP license includes a limit for the number of API users. If you have not signed out, you may have issues with subsequent authorization requests or sign-ins through the API. For details about how to deal with the issues related to the limit for the number of API users during the authorization requests, see the [Troubleshooting](#_3461deb7-e4ae-4f15-b51b-c56883ceb53b) section below.
-   If the application has been granted the api and offline\_access scopes \(that is, the application has requested a refresh token along with an access token\), when the access token has expired, the application can request a new access token by sending a request to the token endpoint and providing the refresh token. Acumatica ERP issues the first access token along with the session ID. If the client application requests a new access token by presenting a refresh token, Acumatica ERP reuses the session ID that was issued for the first access token issued with the refresh token. That is, the system uses a single session for each access granted to the client application. In this case, you do not need to sign out after you have finished your work with Acumatica ERP. This scenario is outside of the scope of this course.
-   If the application has been granted the api:concurrent\_access scope, Acumatica ERP can maintain multiple sessions for the application, managing session IDs through cookies. In this case, the application has to explicitly sign out from Acumatica ERP in each session to close the session. This scenario is outside of the scope of this course.

For details on the scopes that are available for each of the flows, see the descriptions of the flows in the following topics:

-   [Authorization Code Flow: Obtaining of an Authorization Code](#_a8fbe87f-647a-48dd-b713-de6c123a4de4)
-   [Implicit Flow: Obtaining of an Access Token and ID Token](#_c93ae0ae-73f4-4342-bb1f-fe20874d7ed8)
-   [Resource Owner Password Credentials Flow: Obtaining of an Access Token](#_822ddb44-b14f-481a-90c7-f15521e61702)
-   [Hybrid Flow: Obtaining of an Authorization Code, Access Token, and ID Token from the Authorization Endpoint](#_9cd888ba-aa9a-4140-b5e9-4da297fade89)

#### Troubleshooting {#_3461deb7-e4ae-4f15-b51b-c56883ceb53b .section}

You can get the *API Login Limit* error when your application requests access to the Acumatica ERP REST API through OAuth 2.0. For an application that uses OAuth 2.0 for authorization in Acumatica ERP, this error appears if all of the following are true:

-   The API login limit is specified in the Acumatica ERP license. The license restriction for the API users is shown in the **Maximum Number of Web Services API Users** box on the **License** tab of the [License Monitoring Console](UserGuide/SM_60_40_00.md) \(SM604000\) form.
-   The number of unclosed sessions \(that is, the sessions in which you have signed in to Acumatica ERP through the web services API or obtained access to the Acumatica ERP web services API through OAuth 2.0 and have not signed out from Acumatica ERP\) equals the API login limit in the license.
-   You try to request access to the web services API through OAuth 2.0 once more.

You can deal with this error as follows:

1.  Modify the code of your application so that it signs out from Acumatica ERP each time the work with Acumatica ERP is finished.
2.  If the integration application has not closed the session, you can do one of the following:
    -   Pass the access token that was used during the previous session and sign out from Acumatica ERP.
    -   Wait for one hour until the session that has been opened through OAuth 2.0 expires.
    -   Restart the site in the Internet Information Services \(IIS\) Manager or by clicking the **Restart Application** button on the toolbar of the [Apply Updates](UserGuide/SM_20_35_10.md) \(SM203510\) form.

### OAuth 2.0 and OIDC: Assessment Test Questions {#_04642a55-b89e-4717-8370-8a6b631bbd29}

1.  Select all correct statements about the OAuth 2.0 authorization mechanism in Acumatica ERP.
    -   You must register the client application in Acumatica ERP to use the OAuth 2.0 authorization mechanism.
    -   OAuth 2.0 can be used for OData integrations.
    -   With OAuth 2.0, the client application passes the username and password of an Acumatica ERP user each time the application requests data from Acumatica ERP.
    -   OAuth 2.0 requires an HTTPS connection between the client application and Acumatica ERP.
2.  Select a situation in which you must configure an Acumatica ERP website for HTTPS.
    -   The integration application that you are developing needs to use OAuth 2.0 authorization.
    -   The integration application that you are developing uses OData for data retrieval from Acumatica ERP.
    -   The integration application that you are developing uses the contract-based REST API.

**Signing Out from Acumatica ERP**

1.  Select the correct statement about signing out from Acumatica ERP through the contract-based REST API.
    -   You must always sign out from Acumatica ERP in the client application if the application uses cookies to manage the application sessions.
    -   You must always sign out from Acumatica ERP in the client application if the application uses the OAuth 2.0 authorization mechanism.
    -   The limit for the number of API users of an Acumatica ERP license does not affect the OAuth 2.0 authorization requests.
2.  How long can you use the access token if you have granted only the api scope to the application?
    -   Half an hour
    -   One hour
    -   One hour since the last REST API call
    -   During the current Windows session
3.  If you are using the contract-based REST API, how do you sign out from Acumatica ERP?
    -   By calling the POST HTTP method and the endpoint for signing out
    -   By calling the DELETE HTTP method and the endpoint for signing in
    -   By closing the client application
4.  
## Registering Client Applications That Support OAuth 2.0 or OIDC {#_ff95236f-4221-49f7-8cec-9471458f261b}

Before a client application that implements OAuth 2.0 or OpenID Connect \(OIDC\) can work with Acumatica ERP, you need to register the application in the system. In this chapter, you will learn how to register a client application in Acumatica ERP and which options are available during registration.

### Registration of an OAuth 2.0 or OIDC Application: General Information {#_84e7f6c6-3b0f-4526-a1aa-343e39aeebfe}

You use the [Connected Applications](UserGuide/SM_30_30_10.md) \(SM303010\) form to register an OAuth 2.0 or OpenID Connect \(OIDC\) client application.

To register a client application in Acumatica ERP, you need to know the authorization flow that this application implements. For more information on the flows, see [Authorization Code Flow: General Information](#_ff780860-09c2-46c9-bdd7-c6c3b1fc442c), [Implicit Flow: General Information](#_76861c67-265c-46f4-949d-c8d4509c99ec), [Resource Owner Password Credentials Flow: General Information](#_2930d2f7-e081-4d0e-8879-93907ce82607), and [Hybrid Flow: General Information](#_f1bcf512-d676-4d8d-99d7-1a25fd565cdf).

#### Learning Objectives { .section}

In this chapter, you will learn how to register an OAuth 2.0 or OIDC client application in Acumatica ERP.

#### Applicable Scenarios { .section}

You are a developer who is implementing an OAuth 2.0 or OIDC client application. Before this application can work with an Acumatica ERP instance, you need to register the application in this instance.

#### Registration of a Client Application { .section}

You register an OAuth 2.0 or OIDC client application in the Acumatica ERP instance so that the application can work with the instance.

**Attention:**

-   According to the OAuth 2.0 and OIDC specifications, a secure connection between a client application and the Acumatica ERP website with a Secure Sockets Layer \(SSL\) certificate is required. Therefore, you have to set up the Acumatica ERP website for HTTPS before the client application can work with data in Acumatica ERP. For more information, see [Preparation for the Acumatica ERP Installation: System Environment](UserGuide/INST_Preparing_Installation_System_Environment.md).
-   When you are registering the client application, you have to be signed in to the tenant whose data the client application needs to access.

To register a client application, you perform the following general steps on the [Connected Applications](UserGuide/SM_30_30_10.md) \(SM303010\) form:

1.  In the **Client Name** box of the Summary area, you type the name of the client application.

    **Tip:** You should leave the **Client ID** box blank. The system will fill it in when you save your settings on the form.

2.  In the **Flow** box, you select the authorization flow.
3.  Depending on the flow you have selected, you specify the relevant settings, which are listed in the following table. \(*+* indicates that the setting is available for the flow; *−* indicates that the setting is unavailable for the flow.\)

    |Settings|*Authorization Code* Flow|*Implicit* Flow|*Resource Owner Password Credentials* Flow|*Hybrid* Flow|
    |--------|-------------------------|---------------|------------------------------------------|-------------|
    |Mode of refresh token expiration \(in the **Mode** box of the **Refresh Tokens** section of the Summary area\). For details, see [Registration of an OAuth 2.0 or OIDC Application: Sliding Expiration of Refresh Tokens](#_92bf610c-f18c-446c-8e62-5fb928ef2def).|+|−|+|+|
    |Shared secret \(which you add by clicking **Add Shared Secret** on the toolbar of the **Secrets** tab\).|+|−|+|+|
    |JSON Web Key \(which you add by clicking **Add JSON Web Key** on the toolbar of the **Secrets** tab\). For more information, see [Registration of an OAuth 2.0 or OIDC Application: JWT Bearer Tokens](#_a7018c07-aeb9-4803-9dd3-2ccbb4a442f2).|+|−|+|+|
    |JSON Web Key Set URL \(which you add by clicking **Add JSON Web Key Set URL** on the toolbar of the **Secrets** tab\).|+|−|+|+|
    |The redirect URI \(on the **Redirect URIs** tab\).|+|+|−|+|
    |For an OIDC application, the claims that will be included in the ID token \(by selecting or clearing the **Active** check boxes on the **Claims** tab\). For details, see [Registration of an OAuth 2.0 or OIDC Application: Acumatica ERP as an Identity Provider via OIDC](#_e3b36e51-ee22-4b7a-a5d6-2d135a8926e6).|+|+|−|+|
    |For an OIDC application, the plug-in that contains custom claims \(in the **Plug-In** box of the Summary area\).|+|+|−|+|


After the registration, you have the client ID of the client application and, if you have selected a shared secret, the secret value.

### Registration of an OAuth 2.0 or OIDC Application: Sliding Expiration of Refresh Tokens {#_92bf610c-f18c-446c-8e62-5fb928ef2def}

If you do not want a user to reauthorize the client application to work with Acumatica ERP every 30 days, you can configure the sliding expiration of refresh tokens for client applications. On the [Connected Applications](UserGuide/SM_30_30_10.md) \(SM303010\) form, for any client application that has the *Authorization Code*, *Resource Owner Password Credentials*, or *Hybrid* flow, you can select the *Sliding Expiration* mode in the **Refresh Tokens** section in the Summary area. You can also specify the length of the sliding lifetime and indicate whether the refresh tokens for the application have an absolute lifetime.

#### How the Sliding Expiration Works { .section}

When a user grants the offline\_access scope \(along with the api or openid scope\) to a connected application, the application receives a refresh token and an access token. The application can then access data in Acumatica ERP during a specific period of time, which is specified in the response that returns the access token. When the access token expires, the client application can request a new access token by providing the refresh token to the token endpoint. The refresh token can be provided anytime within 30 days of the first issuing of the token.

If during these 30 days, the connected application provides the refresh token to the token endpoint, the system extends the period of time for which the new refresh token is valid. The lifetime is extended by the time that is specified in the **Sliding Lifetime \(Days\)** box in the Summary area \(**Refresh Tokens** section\) of the [Connected Applications](UserGuide/SM_30_30_10.md) \(SM303010\) form. The lifetime of the refresh token can be extended multiple times by the period of the sliding lifetime until the refresh token's total lifetime \(from its initial issuing\) exceeds the number of days that is specified in the **Absolute Lifetime \(Days\)** box. If the **Infinite** check box is selected for the absolute lifetime, the lifetime of the refresh token can be extended endlessly. The following diagram illustrates the sliding expiration of refresh tokens.

![](IntegrationDevelopmentGuide/Images/diag_Sliding_Refresh_Token_Expiration.png "Lifetime of refresh tokens with sliding expiration")

### Registration of an OAuth 2.0 or OIDC Application: JWT Bearer Tokens {#_a7018c07-aeb9-4803-9dd3-2ccbb4a442f2}

Acumatica ERP implements support for JSON Web Token \(JWT\) bearer tokens for client authentication. With this support, the private secret key is stored only in the client application, while the Acumatica ERP instance holds the public key.

#### Registration of the Application { .section}

When you register the application on the [Connected Applications](UserGuide/SM_30_30_10.md) \(SM303010\) form, you add either a JSON Web Key \(JWK\) or a JSON Web Key Set\(JWKS\) URL on the **Secrets** tab.

To add a JWK, you click the new **Add JSON Web Key** button on the table toolbar and specify the needed settings in the dialog box that opens. For JWK, Acumatica ERP supports the format that is defined in RFC7517 \([https://datatracker.ietf.org/doc/html/rfc7517\#section-4](https://nam12.safelinks.protection.outlook.com/?url=https%3A%2F%2Fdatatracker.ietf.org%2Fdoc%2Fhtml%2Frfc7517%23section-4&data=05%7C01%7Ckpopova%40acumatica.com%7C520550d56a7048ac035608dbeac2da1e%7C5ba58136c8e34f4b85797e49a2e3239c%7C0%7C0%7C638361894837514888%7CUnknown%7CTWFpbGZsb3d8eyJWIjoiMC4wLjAwMDAiLCJQIjoiV2luMzIiLCJBTiI6Ik1haWwiLCJXVCI6Mn0%3D%7C3000%7C%7C%7C&sdata=cRvnakLCNvcXxjOzkJ5ulu1PACHkmjS2n4j2IZ0sHs8%3D&reserved=0)\).

To add a JWKS URL, you click the new **Add JSON Web Key Set URL** button on the table toolbar and specify the needed settings in the dialog box that opens. The JWKS URL should point to a location that satisfies the following requirements:

-   It is accessible from each Acumatica ERP instance that is used with the client application. If the location is inaccessible, the token request is declined with the invalid\_client error.
-   It complies with RFC7515 \([https://datatracker.ietf.org/doc/html/rfc7517\#section-5](https://nam12.safelinks.protection.outlook.com/?url=https%3A%2F%2Fdatatracker.ietf.org%2Fdoc%2Fhtml%2Frfc7517%23section-5&data=05%7C01%7Ckpopova%40acumatica.com%7C520550d56a7048ac035608dbeac2da1e%7C5ba58136c8e34f4b85797e49a2e3239c%7C0%7C0%7C638361894837529236%7CUnknown%7CTWFpbGZsb3d8eyJWIjoiMC4wLjAwMDAiLCJQIjoiV2luMzIiLCJBTiI6Ik1haWwiLCJXVCI6Mn0%3D%7C3000%7C%7C%7C&sdata=a%2FkID1tp9EyniS740ElpsO3RoDOiVKCenu8uLw%2BYceQ%3D&reserved=0)\).
-   It should support a reasonable load because each Acumatica ERP instance that is used with the client application will access this location on every token request.

### Registration of an OAuth 2.0 or OIDC Application: Acumatica ERP as an Identity Provider via OIDC {#_e3b36e51-ee22-4b7a-a5d6-2d135a8926e6}

Acumatica ERP can be used as an identity provider via the OpenID Connect \(OIDC\) protocol. An OIDC client application uses the Acumatica ERP sign-in page for authentication. During the first sign-in, the client application requests access to the user attributes; for the application to be signed in, a user must confirm the granting of access to these attributes.

#### Registration of the Application { .section}

On the [Connected Applications](UserGuide/SM_30_30_10.md) \(SM303010\) form, support for OIDC is available for the Authorization Code, Hybrid, and Implicit flows.

On the **Claims** tab of the [Connected Applications](UserGuide/SM_30_30_10.md) form, the check box should be selected in the **Active** column for the claims that will be included in the token in the response to the client application \(when OIDC is used\). By default, Acumatica ERP contains a set of claims and a set of scopes; each scope defines the claims that will be included in a response when the scope is specified in a request. These sets of scopes and claims can be redefined in a plug-in included in a customization project. If a plug-in is selected in the Summary area of the [Connected Applications](UserGuide/SM_30_30_10.md) form, the claims that are defined in this plug-in are added to the table on the **Claims** tab; in this table, these claims are marked as belonging to the plug-in.

### Registration of an OAuth 2.0 or OIDC ApplicationActivity 1.1.1: To Register the Application in Acumatica ERP {#_3ab47faf-6e35-415d-88ba-f8b5a8f8c67e}

This activity will walk you through the process of registering of a connected application on the [Connected Applications](UserGuide/SM_30_30_10.md) \(SM303010\) form.

#### Story { .section}

Suppose that you want to provide secure access of the MyStoreIntegration application to Acumatica ERP through the REST API. You want the MyStoreIntegration application to use the Resource Owner Password Credentials flow. With this flow, the credentials \(username and password\) of an Acumatica ERP user are provided directly to the client application, which uses the credentials to obtain the access token. Before the application can work with an Acumatica ERP instance, you need to register the application in this instance.

#### Process Overview { .section}

To register the MyStoreIntegration application in Acumatica ERP as a connected application that uses the OAuth 2.0 authorization, you will use the [Connected Applications](UserGuide/SM_30_30_10.md) \(SM303010\) form.

#### System Preparation { .section}

Before you begin performing the steps of this activity, do the following:

1.  Deploy a new Acumatica ERP instance with the *T100* dataset. For details on deploying an instance, see [Instance Deployment: To Deploy an Instance with Demo Data](UserGuide/INST_Deploying_Instances_Deploy_Tenant_With_Demodata_Activity.md).
2.  To sign in to the instance in the client application, use the tenant name \(which you specified when you created the instance\) and the *MYSTORE* branch.

When you are registering the client application, you have to be signed in to the tenant whose data the client application needs to access, because the client ID that is generated during the application registration includes the name of the tenant. In the instance that you use for the training course, there is only one tenant configured \(with the name *MyStore*\).

**Attention:** According to the OAuth 2.0 specification, a secure connection between an OAuth 2.0 client application and the Acumatica ERP website with a Secure Socket Layer \(SSL\) certificate is required. You have set up the Acumatica ERP website for HTTPS before you started this course \(as described in [Configuring a Website for HTTPS](Trainings/I320/Prerequisites_HTTPS.md)\).

#### Step: Registering a Connected Application { .section}

Proceed as follows:

1.  In the Summary area of the [Connected Applications](UserGuide/SM_30_30_10.md) \(SM303010\) form, specify the following values:
    -   **Client Name**: `MyStoreIntegration`
    -   **Flow**: *Resource Owner Password Credentials*
2.  On the **Secrets** tab, click **Add Shared Secret**.
3.  In the **Add Shared Secret** dialog box, which opens, do the following:
    1.  In the **Description** box, type `MyStoreIntegration Secret`.
    2.  Copy and save the value from the **Value** box.

        **Attention:** For security reasons, the value of the secret is displayed only once: when you create the secret by invoking this dialog box. Therefore, if you do not save the secret, you will not be able to obtain its value in the future.

    3.  Click **OK**.
4.  On the form toolbar, click **Save**. Notice that the client ID has been generated and inserted in the **Client ID** box. The name of the tenant to which you are signed in is appended to this client ID. The MyStoreIntegration application will use this client ID along with the client secret for authentication in Acumatica ERP.

### Registration of an OAuth 2.0 or OIDC Application: Assessment Test Questions {#_f1990c3a-d7f4-48e8-868a-420518d40b79}

1.  Use 1 of 4 questions

    1.  Select all correct statements about the registration of an OAuth 2.0 client application in Acumatica ERP.
        -   You perform this registration on the Connected Applications \(SM303010\) form.
        -   During the registration, you must be signed in to the tenant whose data the application needs to access.
        -   You can always find the values of the client ID and client secret, which are results of the registration, on the Connected Applications \(SM303010\) form.
        -   You do not need to select the OAuth 2.0 flow during the registration.
    2.  Select all correct statements about the registration of an OAuth 2.0 client application in Acumatica ERP.
        -   You perform this registration on the Connected Applications \(SM303010\) form.
        -   During the registration, you can be signed in to any tenant, no matter which tenant's data the application needs to access.
        -   During the registration of the application, you must specify the OAuth 2.0 flow that the client application will use.
    3.  Select all correct statements about the registration of an OAuth 2.0 client application in Acumatica ERP.
        -   You perform this registration on the External Applications \(SM301000\) form.
        -   The client ID that is generated during the registration includes the name of the tenant.
        -   The client application uses either the client ID or the client secret for authentication in Acumatica ERP.
        -   During the registration, you must specify the OAuth 2.0 flow that the client application will use.
    4.  Select all correct statements about the registration of an OAuth 2.0 client application in Acumatica ERP.
        -   You perform this registration on the Connected Applications \(SM303010\) form.
        -   After the registration, you can retrieve data from only the tenant to which you were signed in during the registration.
        -   The OAuth 2.0 flow setting is optional during the registration.

## Implementing the Authorization Code Flow {#_6c9de791-81bf-4a36-8f20-8076c13c40b3}

The Authorization Code flow is a secure method for authorization used in OAuth 2.0 and OpenID Connect \(OIDC\). This flow is typically used in scenarios where the client application needs to access resources on behalf of the user, but the client application itself is not trusted with the user's credentials. In this chapter, you can find details about the implementation of the Authorization Code flow.

### Authorization Code Flow: General Information {#_ff780860-09c2-46c9-bdd7-c6c3b1fc442c}

When you implement OAuth 2.0 or OpenID Connect \(OIDC\) in a client application to make the application work with Acumatica ERP, you can use the Authorization Code flow. With this authorization flow, the client application never gets the credentials of the applicable Acumatica ERP user. After the user is authenticated in Acumatica ERP, the client application receives an authorization code, exchanges it for an access token, and then uses the access token to work with data in Acumatica ERP.

#### Learning Objectives { .section}

In this chapter, you will learn how to implement a client application that uses the Authorization Code flow.

#### Applicable Scenarios { .section}

You implement the Authorization Code flow in a client application when you want to securely obtain an access token without exposing the user's credentials to the client application.

#### Authorization Code Flow { .section}

For the support of the Authorization Code flow, you implement the following general steps in the application:

1.  **Obtaining an authorization code**

    If the client application uses the proof key for code exchange \(PKCE\), the client application generates the code verifier and the code challenge. For details about this generation, see [https://www.rfc-editor.org/rfc/rfc7636](https://www.rfc-editor.org/rfc/rfc7636). Only the S256 challenge method is supported.

    The client application connects to the authorization endpoint of Acumatica ERP; if PKCE is used, it provides the code challenge.

    The authorization endpoint directs the user of the client application to the sign-in page of Acumatica ERP, where the user should enter the credentials to sign in to a tenant configured in the Acumatica ERP instance.

    **Note:** The user must sign in to the tenant that was specified in the client\_id URL parameter passed to the authorization endpoint. \(This tenant is selected by default on the sign-in page.\)

    If the credentials are accepted by Acumatica ERP, the system displays the consent form, where the user can confirm that the application has access to the requested scopes. Only the scopes that were requested by the application are displayed on the consent form.

    Once the user grants access to the requested scopes, Acumatica ERP redirects the client application to the address that was specified in the request, and adds the authorization code in the URL parameter.

    For details on the request for the authorization code, see [Authorization Code Flow: Obtaining of an Authorization Code](#_a8fbe87f-647a-48dd-b713-de6c123a4de4).

2.  **Obtaining an access token and ID token**

    If the client application uses JSON Web Token \(JWT\) bearer tokens, the application generates a JWT and signs it with the private key.

    The client application connects to the token endpoint of Acumatica ERP and submits the following:

    -   The authorization code.
    -   A signed JWT or a shared secret. If a JWT is provided, Acumatica ERP verifies the JWT signature by using the public key \(which was specified during the registration of the client application in Acumatica ERP\) and validates the JWT payload. If a shared secret is provided, Acumatica ERP verifies the provided application credentials.
    -   If PKCE is used, the code verifier. Acumatica ERP validates the code verifier upon the code challenge that the system has received from the client application with the request for the authorization code.
    If verification is completed successfully, Acumatica ERP issues the access token, the ID token, and the refresh token if these tokens have been requested by the application. The client application should provide the access token with each data request to Acumatica ERP.

    If the ID token is retrieved, the client application validates it by using the key that is available on the [OpenID Connect Preferences](UserGuide/SM_30_30_30.md) \(SM303030\) form. The client application can obtain the key through a `GET` request to the following URL: *\[&lt;Acumatica ERP instance URL&gt;\]/identity/.well-known/openid-configuration/jwks*. The ID token contains the claims to which the user has granted access.

    For more information on this process, see [Authorization Code Flow: Obtaining of an Access Token and ID Token](#_81fe7518-a332-4629-bc63-e3459a116703).

3.  **Optional: Retrieving the user information**

    The client application requests user information from Acumatica ERP and provides the access token with this request. Acumatica ERP returns the information for which the user has provided the consent. For details about this request, see [OAuth 2.0 and OIDC: Obtaining of the User Data](#_065d2515-3e48-4ac6-8c62-56ae546f92eb).

    **Attention:** The recommended way of obtaining the user data is to parse the validated ID token, which contains the same claims as the ones that are obtained through this request.

4.  **Optional: Working with data in Acumatica ERP**

    The client application requests data from Acumatica ERP and provides the access token with this request. Acumatica ERP returns the requested data. For details on this process, see [OAuth 2.0 and OIDC: Working with Data in Acumatica ERP](#_ed9fe5ab-b3c0-444c-8335-ff3b0c8392d6).


When the access token expires, the client application can request a new access token by providing a refresh token, as described in [OAuth 2.0 and OIDC: Refreshing of an Access Token](#_2a5781c6-b661-409c-82f3-8adafd6a332d).

For details on the OAuth 2.0 authorization mechanism, see the specification at [https://tools.ietf.org/html/rfc6749](https://tools.ietf.org/html/rfc6749). For details on the OIDC authorization mechanism, see the specification at [https://openid.net/specs/openid-connect-core-1\_0.html\#Authentication](https://openid.net/specs/openid-connect-core-1_0.html#Authentication).

**Tip:** The configuration of the OpenID Connect protocol that is used by an Acumatica ERP website can be displayed by a request to the following URL: *\[&lt;Acumatica ERP instance URL&gt;\]/identity/.well-known/openid-configuration*. \(In this request, *\[&lt;Acumatica ERP instance URL&gt;\]* stands for the URL of the Acumatica ERP website.\)

#### Authorization Code Flow Diagram { .section}

The following diagram illustrates the Authorization Code flow.

![](IntegrationDevelopmentGuide/Images/diag_AuthorizationCodeFlow.png "Authorization Code flow")

### Authorization Code Flow: Obtaining of an Authorization Code {#_a8fbe87f-647a-48dd-b713-de6c123a4de4}

To obtain an authorization code, the client application connects to the authorization endpoint of Acumatica ERP with the `GET` HTTP method and specifies the parameters of the request in the URL. For details on the request and the response, see the following sections.

#### HTTP Method and URL { .section}

The client application connects to the authorization endpoint of Acumatica ERP with the `GET` method. The client application can use one of the following options for the URL:

-   If the client application supports OpenID Connect Discovery, the client application can use the discovery endpoint address, as shown in the following code.

    ```
    https://<Acumatica ERP instance URL>/identity/.well-known/openid-configuration
    ```

    **Tip:** We recommend that the client application use the discovery endpoint address to obtain the token endpoint address. The use of the discovery endpoint eliminates the need to change the application if the address of the token endpoint changes.

    **Attention:** A request to the discovery endpoint does not provide the access token; it provides the address of the token endpoint from which you can receive the access token.

-   The client application can directly use the authorization endpoint address, which is shown in the following code.

    ```
    https://<Acumatica ERP instance URL>/identity/connect/authorize
    ```


#### Parameters { .section}

The client application should specify the following URL parameters.

<table id="_12da1ab5-251a-4abd-ac7f-f8ce11c170bb"><thead><tr><th>

Parameter

</th><th>

Description

</th></tr></thead><tbody><tr><td>

response\_type

</td><td>

The type of the flow, which must be set to `code` for the Authorization Code flow.

</td></tr><tr><td>

client\_id

</td><td>

The client ID that was assigned to the client application during the registration of the application in Acumatica ERP. The client ID must have the format in which the ID was generated during the registration of the application. That is, the client ID must include an auto-generated string and the ID of the tenant, such as *88358B02-A48D-A50E-F710-39C1636C30F6@MyTenant*. The client application will have access to the data of the tenant specified in the client ID.

</td></tr><tr><td>

redirect\_uri

</td><td>

The URI in the client application to which the response to the request should be sent. The URI must exactly match one of the values specified for the application in the **Redirect URI** column on the **Redirect URIs** tab of the [Connected Applications](UserGuide/SM_30_30_10.md#) \(SM303010\) form.

</td></tr><tr><td>

scope

</td><td>

The access scope that is requested by the client application. The scope can be a combination of the following values, delimited by spaces:

 -   openid: Requests access to the personal information of the user. If this scope is granted, the OpenID Connect authorization mechanism is used. Without this scope, OAuth 2.0 is used.
-   email: Requests disclosure of the user's email address.
-   profile: Requests disclosure of the user's profile information.
-   phone: Requests disclosure of the user's phone number.
-   api: Requests access to the REST API, screen-based SOAP API, and OData interface.

If this scope is granted and the api:concurrent\_access scope is not granted, Acumatica ERP manages the sessions of the application through tokens. Acumatica ERP issues the first access token along with the session ID. If the client application requests a new access token by presenting a refresh token, Acumatica ERP reuses the session ID that was issued for the first access token issued with the refresh token. That is, the system uses a single session for each access granted to the client application. For details about the license limitations related to the number of sessions for client applications, see [License Restrictions for API Users](IntegrationDevelopmentGuide/IS__con_License_Restrictions_API_Users.md).

-   offline\_access: Requests that a refresh token be granted. If a user grants this scope to the application, Acumatica ERP issues to the client application a refresh token along with the access token. When the access token has expired, the client application can request a new access token by sending a request to the token endpoint and providing the refresh token. By default, the whole chain for the refresh token expires 30 days after the initial authentication process. However, you can change these settings in the **Refresh Tokens** section of the Summary area of the [Connected Applications](UserGuide/SM_30_30_10.md) \(SM303010\) form. For details, see [Registration of an OAuth 2.0 or OIDC Application: Sliding Expiration of Refresh Tokens](#_92bf610c-f18c-446c-8e62-5fb928ef2def).
-   api:concurrent\_access: Requests permission for the concurrent use of multiple types of web service APIs. If a user grants this scope to the application, the client application can access data in Acumatica ERP in concurrent mode. In this case, Acumatica ERP can maintain multiple sessions for the client application, managing session IDs through cookies. We recommend that the client application request this scope only if concurrent access is required for the client application. For details about the license limitations related to the number of sessions for client applications, see [License Restrictions for API Users](IntegrationDevelopmentGuide/IS__con_License_Restrictions_API_Users.md).

</td></tr><tr><td>

code\_challenge\_method

</td><td>

For a client application that uses the proof key for code exchange \(PKCE\), the code challenge method, which must be set to `S256`.

</td></tr><tr><td>

code\_challenge

</td><td>

For a client application that uses PKCE, the code challenge. The code challenge is the base64url-encoded SHA-256 hash of the code verifier. The code verifier is a cryptographically random string that is used to correlate the authorization request to the token request. For details about the code verifier and the code challenge, see [https://www.rfc-editor.org/rfc/rfc7636](https://www.rfc-editor.org/rfc/rfc7636).

</td></tr></tbody>
</table>#### Response { .section}

Once the user grants access to the requested scopes, Acumatica ERP redirects the client application to the redirect\_uri address that was specified in the request, and adds the authorization code in the code URL parameter.

#### Example { .section}

An example of a request to the authorization endpoint is shown below. \(Line breaks are for display purposes only.\)

```
GET https://localhost/AcumaticaDB/identity/connect/authorize?
response_type=code
&client_id=58FCCFBD-0CF3-C047-B720-A631C976A8DD@U100
&redirect_uri=http%3A%2F%2Flocalhost%2Fclientapp%2F
&scope=api%20offline_access
```

Once the user grants access to the requested scopes, Acumatica ERP redirects the client application to the following URL: *https://localhost/clientapp/?code=rOBVT0nmPhaXlHeBpE81iJBrfIt5r7ud5\_2czGYIr14&amp;scope=api%20offline\_access*.

### Authorization Code Flow: Obtaining of an Access Token and ID Token {#_81fe7518-a332-4629-bc63-e3459a116703}

To obtain an access token, an ID token, or both, a client application that implements the Authorization Code flow connects to the token endpoint of Acumatica ERP with the `POST` method. For details on the request and the response, see the following sections.

#### HTTP Method and URL {#_76d8934a-3c19-4ee8-b537-005719690be4 .section}

The client application connects to the token endpoint of Acumatica ERP with the `POST` method. The client application can use one of the following options for the URL:

-   If the client application supports OpenID Connect Discovery, the client application can use the discovery endpoint address, as shown in the following code.

    ```
    https://<Acumatica ERP instance URL>/identity/.well-known/openid-configuration
    ```

    **Tip:** We recommend that the client application use the discovery endpoint address to obtain the token endpoint address. The use of the discovery endpoint eliminates the need to change the application if the address of the token endpoint changes.

    **Attention:** A request to the discovery endpoint does not provide the access token; it provides the address of the token endpoint from which you can receive the access token.

-   The client application can directly use the token endpoint address, as shown in the following code.

    ```
    https://<Acumatica ERP instance URL>/identity/connect/token
    ```


#### HTTP Header { .section}

You use the following HTTP header.

|Key|Value|
|---|-----|
|`Content-Type`|`application/x-www-form-urlencoded`|

#### Request Body { .section}

You specify the following parameters in the request body.

<table id="_3c6af2e6-02dd-432c-990e-5d5a629cd17b"><thead><tr><th>

Parameter

</th><th>

Description

</th></tr></thead><tbody><tr><td>

grant\_type

</td><td>

The type of the OAuth 2.0 flow, which must be set to *authorization\_code* for the Authorization Code flow.

</td></tr><tr><td>

client\_id

</td><td>

The client ID that was assigned to the client application during the registration of the application in Acumatica ERP. The client ID must have the format in which the ID was generated during the registration of the application. That is, the client ID must include an auto-generated string and the ID of the tenant, such as *88358B02-A48D-A50E-F710-39C1636C30F6@MyTenant*. The client application will have access to the data of the tenant specified in the client ID.

</td></tr><tr><td>

code

</td><td>

The authorization code that the client application has received from the authorization endpoint.

</td></tr><tr><td>

client\_secret

</td><td>

For a client application that uses a shared secret, the value of the secret that was created for the client application during the registration of the application in Acumatica ERP.

</td></tr><tr><td>

client\_assertion\_type

</td><td>

For a client application that uses JSON Web Token \(JWT\) bearer tokens, the client assertion type, which must be set to *urn:ietf:params:oauth:client-assertion-type:jwt-bearer*.

</td></tr><tr><td>

client\_assertion

</td><td>

For a client application that uses JSON Web Token \(JWT\) bearer tokens, a single JWT.

</td></tr><tr><td>

code\_verifier

</td><td>

For a client application that uses the proof key for code exchange \(PKCE\), the code verifier for which the client application sent the code challenge during the request for the authorization code. For details about the code verifier and the code challenge, see [https://www.rfc-editor.org/rfc/rfc7636](https://www.rfc-editor.org/rfc/rfc7636).

</td></tr><tr><td>

redirect\_uri

</td><td>

The URI in the client application to which the response to the request should be sent. The URI must exactly match one of the values specified for the application in the **Redirect URI** column on the **Redirect URIs** tab of the [Connected Applications](UserGuide/SM_30_30_10.md#) \(SM303010\) form.

</td></tr></tbody>
</table>#### Response { .section}

Acumatica ERP verifies the provided application credentials and issues an access token, an ID token, and a refresh token if they have been requested by the application. The client application should provide the access token with each data request to Acumatica ERP.

A successful response includes the following parameters in the response body.

<table id="_dc2096a3-af1b-49f8-a580-f26674cc082d"><thead><tr><th>

Parameter

</th><th>

Description

</th></tr></thead><tbody><tr><td>

token\_type

</td><td>

The type of the access token, which is *Bearer*. The parameter is returned only if the api scope was granted.

</td></tr><tr><td>

access\_token

</td><td>

The access token. The parameter is returned only if the api scope was granted.

</td></tr><tr><td>

expires\_in

</td><td>

The period of time \(in seconds\) during which the access token is valid. The parameter is returned only if the api scope was granted.

</td></tr><tr><td>

scope

</td><td>

The scope for which the access token and ID token are provided. The returning of this parameter is optional.

</td></tr><tr><td>

refresh\_token

</td><td>

The refresh token. The parameter is returned only if the offline\_access scope was granted.

</td></tr><tr><td>

id\_token

</td><td>

The ID token associated with the authenticated session. The ID token contains three parts, which are separated by periods. The parts are Base64 encoded. The second part contains the claims to which the user granted access. For details on the ID token structure, see [https://openid.net/specs/openid-connect-core-1\_0.html\#IDToken](https://openid.net/specs/openid-connect-core-1_0.html#IDToken) and [https://www.rfc-editor.org/rfc/rfc7519.html](https://www.rfc-editor.org/rfc/rfc7519.html). We recommend that you use the existing standard libraries for parsing the tokens. The parameter is returned only if the openid scope was granted.

</td></tr></tbody>
</table>#### Example { .section}

The following example shows a request for an access token with a shared secret provided with the request. \(Line breaks are for display purposes only.\)

```
POST /identity/connect/token HTTP/1.1
Host: https://localhost/AcumaticaDB
Content-Type: application/x-www-form-urlencoded

grant_type=authorization_code
&code=rOBVT0nmPhaXlHeBpE81iJBrfIt5r7ud5_2czGYIr14
&client_id=58FCCFBD-0CF3-C047-B720-A631C976A8DD@U100
&client_secret=cTUa8QxZnloGoxpT_u3ZBA
&redirect_uri=https%3A%2F%2Flocalhost
```

A successful response has the body shown in the following example.

```
{
    "access_token": "u39uoZj9A4fj2T80Zx0Qirznr0oqNb1qK92c48ZdxUg",
    "expires_in": 3600,
    "token_type": "Bearer",
    "scope": "api offline_access"
}
```

## Implementing the Implicit Flow {#_3141f521-3072-4345-b0aa-c734675af2ea}

The Implicit flow is a type of OAuth 2.0 or OpenID Connect \(OIDC\) flow that is primarily used when the client application \(typically a web application running in a browser\) is incapable of keeping secrets confidential. In this flow, the access token is returned directly to the client application after authentication, without an intermediate step to exchange authorization code. In this chapter, you can find details about the implementation of the Implicit flow.

### Implicit Flow: General Information {#_76861c67-265c-46f4-949d-c8d4509c99ec}

When you implement OAuth 2.0 or OpenID Connect \(OIDC\) in a client application to make the application work with Acumatica ERP, you can use the Implicit flow, which is a simplified variant of the Authorization Code flow.

With the Implicit flow, the client application never gets the credentials of the applicable Acumatica ERP user. When the user is authenticated in Acumatica ERP, the client application does not receive an authorization code \(as with the Authorization Code flow\); instead, the client application directly receives an access token, and then uses the access token to work with data in Acumatica ERP. The access token is valid for a limited period of time and cannot be renewed.

#### Learning Objectives { .section}

In this chapter, you will learn how to implement a client application that uses the Implicit flow.

#### Applicable Scenarios { .section}

You implement the Implicit flow in a client application when you want to securely obtain an access token without exposing the user's credentials to the client application. This flow can be used for clients using a scripting language \(such as JavaScript\) or for mobile clients.

#### Implicit Flow { .section}

For the support of the Implicit flow, you implement the following general steps in the application:

1.  **Obtaining an access token**

    The client application connects to the authorization endpoint of Acumatica ERP.

    The authorization endpoint directs the user of the client application to the sign-in page of Acumatica ERP, where the user should enter the credentials to sign in to a tenant configured in the Acumatica ERP instance.

    **Note:** The user must sign in to the tenant that was specified in the client\_id URL parameter passed to the authorization endpoint. \(This tenant is selected by default on the sign-in page.\)

    If the credentials are accepted by Acumatica ERP, the system displays the consent form, where the user can confirm that the application has access to the requested scopes. Only the scopes that were requested by the application are displayed on the consent form.

    Once the user grants access to the requested scopes, Acumatica ERP issues the access token and the ID token \(if requested\). The client application should provide the access token with each data request to Acumatica ERP.

    If the ID token is retrieved, the client application validates it by using the key that is available on the [OpenID Connect Preferences](UserGuide/SM_30_30_30.md) \(SM303030\) form. The client application can obtain the key through a `GET` request to the following URL: *\[&lt;Acumatica ERP instance URL&gt;\]/identity/.well-known/openid-configuration/jwks*. The ID token contains the claims to which the user has granted access.

    For more information on the request that obtains the tokens, see [Implicit Flow: Obtaining of an Access Token and ID Token](#_c93ae0ae-73f4-4342-bb1f-fe20874d7ed8).

2.  **Optional: Retrieving the user information**

    The client application requests user information from Acumatica ERP and provides the access token with this request. Acumatica ERP returns the information for which the user has provided the consent. For details about this request, see [OAuth 2.0 and OIDC: Obtaining of the User Data](#_065d2515-3e48-4ac6-8c62-56ae546f92eb).

    **Attention:** The recommended way of obtaining the user data is to parse the validated ID token, which contains the same claims as the ones that are obtained through this request.

3.  **Optional: Working with data in Acumatica ERP**

    The client application requests data from Acumatica ERP and provides the access token with this request. Acumatica ERP returns the requested data. For details on this process, see [OAuth 2.0 and OIDC: Working with Data in Acumatica ERP](#_ed9fe5ab-b3c0-444c-8335-ff3b0c8392d6).


**Note:** Refresh tokens are not supported by the Implicit flow.

For details on the OAuth 2.0 authorization mechanism, see the specification at [https://tools.ietf.org/html/rfc6749](https://tools.ietf.org/html/rfc6749). For details on the OIDC authorization mechanism, see the specification at [https://openid.net/specs/openid-connect-core-1\_0.html\#Authentication](https://openid.net/specs/openid-connect-core-1_0.html#Authentication).

**Tip:** The configuration of the OpenID Connect protocol that is used by an Acumatica ERP website can be displayed by a request to the following URL: *\[&lt;Acumatica ERP instance URL&gt;\]/identity/.well-known/openid-configuration*. \(In this request, *\[&lt;Acumatica ERP instance URL&gt;\]* stands for the URL of the Acumatica ERP website.\)

#### Implicit Flow Diagram { .section}

The following diagram illustrates the Implicit flow.

![](IntegrationDevelopmentGuide/Images/diag_ImplicitFlow.png "Implicit flow")

### Implicit Flow: Obtaining of an Access Token and ID Token {#_c93ae0ae-73f4-4342-bb1f-fe20874d7ed8}

To obtain an access token, an ID token, or both, a client application that implements the Implicit flow connects to the authorization endpoint of Acumatica ERP with the `GET` method. For details on the request and the response, see the following sections.

#### HTTP Method and URL { .section}

The client application connects to the authorization endpoint of Acumatica ERP with the `GET` method. The client application can use one of the following options for the URL:

-   If the client application supports OpenID Connect Discovery, the client application can use the discovery endpoint address, as shown in the following code.

    ```
    https://<Acumatica ERP instance URL>/identity/.well-known/openid-configuration
    ```

    **Tip:** We recommend that the client application use the discovery endpoint address to obtain the token endpoint address. The use of the discovery endpoint eliminates the need to change the application if the address of the token endpoint changes.

    **Attention:** A request to the discovery endpoint does not provide the access token; it provides the address of the token endpoint from which you can receive the access token.

-   The client application can directly use the authorization endpoint address, which is shown in the following code.

    ```
    https://<Acumatica ERP instance URL>/identity/connect/authorize
    ```


#### Parameters { .section}

The client application should specify the following URL parameters.

<table id="_d3e143b7-85b9-4f88-a0cb-a3d9ae7c616a"><thead><tr><th>

Parameter

</th><th>

Description

</th></tr></thead><tbody><tr><td>

response\_type

</td><td>

The type of the flow. The type can be one of the following:

 -   *token*: Is used for OAuth 2.0 to retrieve an access token. You must include *api* in the scope parameter.
-   *id\_token*: Is used for OIDC to retrieve an ID token. No access token is returned. Do not include *api* in the scope parameter.
-   *id\_token token*: Is used for OIDC to retrieve an ID token and access token. You must include *api* in the scope parameter.

</td></tr><tr><td>

client\_id

</td><td>

The client ID that was assigned to the client application during the registration of the application in Acumatica ERP. The client ID must have the format in which the ID was generated during the registration of the application. That is, the client ID must include an auto-generated string and the ID of the tenant, such as *88358B02-A48D-A50E-F710-39C1636C30F6@MyTenant*. The client application will have access to the data of the tenant specified in the client ID.

</td></tr><tr><td>

redirect\_uri

</td><td>

The URI in the client application to which the response to the request should be sent. The URI must exactly match one of the values specified for the application in the **Redirect URI** column on the **Redirect URIs** tab of the [Connected Applications](UserGuide/SM_30_30_10.md#) \(SM303010\) form.

</td></tr><tr><td>

scope

</td><td>

The access scope that is requested by the client application. The scope can be a combination of the following values, delimited by spaces:

 -   openid: Requests access to the personal information of the user. If this scope is granted, the OpenID Connect authorization mechanism is used. Without this scope, OAuth 2.0 is used.
-   email: Requests disclosure of the user's email address.
-   profile: Requests disclosure of the user's profile information.
-   phone: Requests disclosure of the user's phone number.
-   api: Requests access to the REST API, screen-based SOAP API, and OData interface.

**Important:** The api scope is required if the *token* or *id\_token token* response type is specified.

If this scope is granted and the api:concurrent\_access scope is not granted, Acumatica ERP manages the sessions of the application through tokens. The system uses a single session for each access granted to the client application.

-   api:concurrent\_access: Requests permission for the concurrent use of multiple types of web service APIs. If a user grants this scope to the application, the client application can access data in Acumatica ERP in concurrent mode. In this case, Acumatica ERP can maintain multiple sessions for the client application, managing session IDs through cookies. We recommend that the client application request this scope only if concurrent access is required for the client application. For details about the license limitations related to the number of sessions for client applications, see [License Restrictions for API Users](IntegrationDevelopmentGuide/IS__con_License_Restrictions_API_Users.md).

 **Note:** The offline\_access scope is not supported for the Implicit flow.

</td></tr><tr><td>

nonce

</td><td>

A string value that is used to associate a client session with an ID token, and to mitigate replay attacks. This parameter is required if the *id\_token* or *id\_token token* response type is specified.

</td></tr></tbody>
</table>#### Response { .section}

Once the user grants access to the requested scopes, Acumatica ERP redirects the client application to the redirect\_uri address, which was specified in the request, and adds the requested data in the fragment section of the redirect URL. The redirect URL includes the following fragment parameters.

<table id="_dc2096a3-af1b-49f8-a580-f26674cc082d"><thead><tr><th>

Parameter

</th><th>

Description

</th></tr></thead><tbody><tr><td>

token\_type

</td><td>

The type of the access token, which is *Bearer*. The parameter is returned only if the api scope was granted.

</td></tr><tr><td>

access\_token

</td><td>

The access token. The parameter is returned only if the api scope was granted.

</td></tr><tr><td>

expires\_in

</td><td>

The period of time \(in seconds\) during which the access token is valid. The parameter is returned only if the api scope was granted.

</td></tr><tr><td>

scope

</td><td>

The scope for which the access token and ID token are provided. The returning of this parameter is optional.

</td></tr><tr><td>

id\_token

</td><td>

The ID token associated with the authenticated session. The ID token contains three parts, which are separated by periods. The parts are Base64 encoded. The second part contains the claims to which the user granted access. For details on the ID token structure, see [https://openid.net/specs/openid-connect-core-1\_0.html\#IDToken](https://openid.net/specs/openid-connect-core-1_0.html#IDToken) and [https://www.rfc-editor.org/rfc/rfc7519.html](https://www.rfc-editor.org/rfc/rfc7519.html). We recommend that you use the existing standard libraries for parsing the tokens. The parameter is returned only if the openid scope was granted.

</td></tr></tbody>
</table>#### Example: openid, email, and api Scopes { .section}

The following example requests the *openid*, *email*, and *api* scopes. \(Line breaks are for display purposes only.\)

```
GET https://localhost/AcumaticaDB/identity/connect/authorize?
response_type=id_token%20token
&client_id=2B0C8CF1-FFD4-A0DE-1673-F03084F16240@U100
&redirect_uri=https://localhost
&scope=openid%20email%20api
&nonce=test
```

Once the user grants access to the requested scopes, Acumatica ERP redirects the client application to the following URL.

```
https://localhost/#
id_token=ey...rvE
&access_token=BTGTm5nSGIZWoypYv_QOjD00ziczKaiMEDIVcNf6XpM
&token_type=Bearer
&expires_in=3600
&scope=openid%20email%20api
```

#### Example: openid and email Scopes { .section}

The following example requests the *openid* and *email* scopes. \(Line breaks are for display purposes only.\)

```
GET https://localhost/AcumaticaDB/identity/connect/authorize?
response_type=id_token
&client_id=8F41DD85-CA55-8518-8464-4C983D64BBA4@U100
&redirect_uri=https://localhost
&scope=openid%20email
&nonce=test
```

Once a user grants access to the requested scope, Acumatica ERP redirects the client application to the following URL.

```
https://localhost/#
id_token=eyJh...3YNA
&scope=openid
```

#### Example: api Scope { .section}

The following example requests the *api* scope. \(Line breaks are for display purposes only.\)

```
GET http://localhost/AcumaticaDB/identity/connect/authorize?
response_type=token
&client_id=8F41DD85-CA55-8518-8464-4C983D64BBA4@U100
&redirect_uri=https://localhost
&scope=api
```

Once the user grants access to the requested scope, Acumatica ERP redirects the client application to the following URL.

```
https://localhost/#
access_token=7qgkxo-tJTbouSs8OtU3gdNhW0YQZVA9n6ZQt364nks
&token_type=Bearer
&expires_in=3600
&scope=api
```

## Implementing the Resource Owner Password Credentials Flow {#_01d3f2ed-0856-41db-84e5-e83f8f57986f}

The Resource Owner Password Credentials flow in OAuth 2.0 is used when the client application can obtain the user's username and password and directly exchange them for an access token. Unlike other OAuth 2.0 flows—where the client application interacts with the authorization server through redirections, callbacks, and authorization codes—the Resource Owner Password Credentials flow involves sending the user's credentials directly to the authorization server.

In this chapter, you can find details about the implementation of the Resource Owner Password Credentials flow.

### Resource Owner Password Credentials Flow: General Information {#_2930d2f7-e081-4d0e-8879-93907ce82607}

When you implement OAuth 2.0 in a client application to make the application work with Acumatica ERP, you can use the Resource Owner Password Credentials flow.

With the Resource Owner Password Credentials flow, the credentials \(username and password\) of the Acumatica ERP user are provided directly to the client application, which uses the credentials to obtain the access token. When the access token expires, the client application can request a new access token by providing a refresh token.

#### Learning Objectives { .section}

In this chapter, you will learn how to implement a client application that uses the Resource Owner Password Credentials flow.

#### Applicable Scenarios { .section}

You can use the Resource Owner Password Credentials flow in environments where the client application can securely store user credentials and there is a high level of trust between the user and the client application, as with native mobile applications.

**Attention:** The Resource Owner Password Credentials flow has significant drawbacks and security concerns, such as the following:

-   Handling and transmitting user credentials directly from the client application to the authorization server can introduce security risks.
-   Because the Resource Owner Password Credentials flow bypasses the authorization step, a user does not have the opportunity to review and grant consent to the client application's access to the resources.

Therefore, you should carefully consider the use of the Resource Owner Password Credentials flow, and prefer other authorization flows, such as Authorization Code flow, whenever possible.

#### Resource Owner Password Credentials Flow { .section}

For the support of the Resource Owner Password Credentials flow, you implement the following general steps in the application:

1.  **Obtaining an access token**

    The client application obtains the username and password of the applicable Acumatica ERP user, which can then be exchanged for an access token.

    If the client application uses JSON Web Token \(JWT\) bearer tokens, the application generates a JWT and signs it with the private key.

    The client application connects to the token endpoint of Acumatica ERP, submits the user credentials, and provides a signed JWT or a shared secret. If a JWT is provided, Acumatica ERP verifies the JWT signature by using the public key \(which was specified during the registration of the client application in Acumatica ERP\) and validates the JWT payload. If a shared secret is provided, Acumatica ERP verifies the provided application credentials.

    If verification is completed successfully, Acumatica ERP issues an access token, which the client application should provide with each data request to Acumatica ERP, and a refresh token \(if requested\).

    For more information on this process, see [Resource Owner Password Credentials Flow: Obtaining of an Access Token](#_822ddb44-b14f-481a-90c7-f15521e61702).

2.  **Optional: Working with data in Acumatica ERP**

    The client application requests data from Acumatica ERP and provides the access token with this request. Acumatica ERP returns the requested data. For details on this process, see [OAuth 2.0 and OIDC: Working with Data in Acumatica ERP](#_ed9fe5ab-b3c0-444c-8335-ff3b0c8392d6).


When the access token expires, the client application can request a new access token by providing a refresh token, as described in [OAuth 2.0 and OIDC: Refreshing of an Access Token](#_2a5781c6-b661-409c-82f3-8adafd6a332d).

For details on the OAuth 2.0 authorization mechanism, see the specification at [https://tools.ietf.org/html/rfc6749](https://tools.ietf.org/html/rfc6749).

#### Diagram of the Resource Owner Password Credentials Flow { .section}

The following diagram illustrates the Resource Owner Password Credentials flow.

![](IntegrationDevelopmentGuide/Images/diag_ResourceOwnerPasswordCredentialsFlow.png "Resource Owner Password Credentials flow")

### Resource Owner Password Credentials Flow: Obtaining of an Access Token {#_822ddb44-b14f-481a-90c7-f15521e61702}

To obtain an access token, a client application that implements the Resource Owner Password Credentials flow connects to the token endpoint of Acumatica ERP with the `POST` method. For details on the request and the response, see the following sections.

#### HTTP Method and URL { .section}

The client application connects to the token endpoint of Acumatica ERP with the `POST` method. The client application can use one of the following options for the URL:

-   If the client application supports OpenID Connect Discovery, the client application can use the discovery endpoint address, as shown in the following code.

    ```
    https://<Acumatica ERP instance URL>/identity/.well-known/openid-configuration
    ```

    **Tip:** We recommend that the client application use the discovery endpoint address to obtain the token endpoint address. The use of the discovery endpoint eliminates the need to change the application if the address of the token endpoint changes.

    **Attention:** A request to the discovery endpoint does not provide the access token; it provides the address of the token endpoint from which you can receive the access token.

-   The client application can directly use the token endpoint address, as shown in the following code.

    ```
    https://<Acumatica ERP instance URL>/identity/connect/token
    ```


#### HTTP Header { .section}

You use the following HTTP header.

|Key|Value|
|---|-----|
|`Content-Type`|`application/x-www-form-urlencoded`|

#### Request Body { .section}

You specify the following parameters in the request body.

<table id="_50473d43-1f01-415d-925b-71d935cb18c9"><thead><tr><th>

Parameter

</th><th>

Description

</th></tr></thead><tbody><tr><td>

grant\_type

</td><td>

The type of the OAuth 2.0 flow, which must be set to `password` for the resource owner password credentials flow.

</td></tr><tr><td>

client\_id

</td><td>

The client ID that was assigned to the client application during the registration of the application in Acumatica ERP. The client ID must have the format in which the ID was generated during the registration of the application. That is, the client ID must include an auto-generated string and the ID of the tenant, such as *88358B02-A48D-A50E-F710-39C1636C30F6@MyTenant*. The client application will have access to the data of the tenant specified in the client ID.

</td></tr><tr><td>

client\_secret

</td><td>

For a client application that uses a shared secret, the value of the secret that was created for the client application during the registration of the application in Acumatica ERP.

</td></tr><tr><td>

client\_assertion\_type

</td><td>

For a client application that uses JSON Web Token \(JWT\) bearer tokens, the client assertion type, which must be set to *urn:ietf:params:oauth:client-assertion-type:jwt-bearer*.

</td></tr><tr><td>

client\_assertion

</td><td>

For a client application that uses JSON Web Token \(JWT\) bearer tokens, a single JWT.

</td></tr><tr><td>

username

</td><td>

The username of an Acumatica ERP user.

</td></tr><tr><td>

password

</td><td>

The password for the specified username.

</td></tr><tr><td>

scope

</td><td>

The access scope that is requested by the client application. The scope can be a combination of the following values, delimited by spaces:

 -   api: Requests access to the REST API, screen-based SOAP API, and OData interface.

If this scope is granted and the api:concurrent\_access scope is not granted, Acumatica ERP manages the sessions of the application through tokens. Acumatica ERP issues the first access token along with the session ID. If the client application requests a new access token by presenting a refresh token, Acumatica ERP reuses the session ID that was issued for the first access token issued with the refresh token. That is, the system uses a single session for each access granted to the client application. For details about the license limitations related to the number of sessions for client applications, see [License Restrictions for API Users](IntegrationDevelopmentGuide/IS__con_License_Restrictions_API_Users.md).

-   offline\_access: Requests that a refresh token be granted. If a user grants this scope to the application, Acumatica ERP issues to the client application a refresh token along with the access token. When the access token has expired, the client application can request a new access token by sending a request to the token endpoint and providing the refresh token. By default, the whole chain for the refresh token expires 30 days after the initial authentication process. However, you can change these settings in the **Refresh Tokens** section of the Summary area of the [Connected Applications](UserGuide/SM_30_30_10.md) \(SM303010\) form. For details, see [Registration of an OAuth 2.0 or OIDC Application: Sliding Expiration of Refresh Tokens](#_92bf610c-f18c-446c-8e62-5fb928ef2def).
-   api:concurrent\_access: Requests permission for the concurrent use of multiple types of web service APIs. If a user grants this scope to the application, the client application can access data in Acumatica ERP in concurrent mode. In this case, Acumatica ERP can maintain multiple sessions for the client application, managing session IDs through cookies. We recommend that the client application request this scope only if concurrent access is required for the client application. For details about the license limitations related to the number of sessions for client applications, see [License Restrictions for API Users](IntegrationDevelopmentGuide/IS__con_License_Restrictions_API_Users.md).

</td></tr></tbody>
</table>#### Response { .section}

Acumatica ERP verifies the provided application credentials and issues the access token, which the client application should provide with each data request to Acumatica ERP.

A successful response includes the following parameters in the response body.

<table id="_dc2096a3-af1b-49f8-a580-f26674cc082d"><thead><tr><th>

Parameter

</th><th>

Description

</th></tr></thead><tbody><tr><td>

token\_type

</td><td>

The type of the access token, which is *Bearer*. The parameter is returned only if the api scope was granted.

</td></tr><tr><td>

access\_token

</td><td>

The access token. The parameter is returned only if the api scope was granted.

</td></tr><tr><td>

expires\_in

</td><td>

The period of time \(in seconds\) during which the access token is valid. The parameter is returned only if the api scope was granted.

</td></tr><tr><td>

scope

</td><td>

The scope for which the access token is valid.

</td></tr><tr><td>

refresh\_token

</td><td>

The refresh token. The parameter is returned only if the offline\_access scope was granted.

</td></tr></tbody>
</table>#### Example { .section}

An example of a request is shown below. \(Line breaks are for display purposes only.\)

```
POST /identity/connect/token HTTP/1.1
Host: https://localhost/AcumaticaDB
Content-Type: application/x-www-form-urlencoded

grant_type=password
&client_id=8E0761D9-F4EC-2D4B-A60F-BCE2708C6FDD%40U100
&client_secret=O19LLT5Z0SzFbCIKLXLqQQ
&username=admin
&password=123
&scope=api%20offline_access
```

A successful response has the body shown in the following example.

```
{
    "access_token": "u39uoZj9A4fj2T80Zx0Qirznr0oqNb1qK92c48ZdxUg",
    "expires_in": 3600,
    "token_type": "Bearer",
    "scope": "api offline_access"
}
```

### Resource Owner Password Credentials FlowActivity 1.2.1: To Configure a REST Application to Use OAuth 2.0 {#_638b2743-8ca8-49a8-96d5-e2f0601b0827}

This activity will walk you through the process of configuring a Postman collection to use the OAuth 2.0 authorization for the requests to Acumatica ERP.

#### Story { .section}

Suppose that you need to configure the MyStoreIntegration application, which is a Postman collection, to use the Resource Owner Password Credentials flow.

#### Process Overview { .section}

You will connect to the token endpoint, pass the client ID and client secret in the authorization header, and request access to the web service APIs \(that is, you will request the api scope\). You will receive the access token from Acumatica ERP to use it in subsequent requests to Acumatica ERP. You will not request the refresh token, which the client application can use to request a new access token when the access token has expired.

**Tip:** In Postman, you cannot use the discovery endpoint, because Postman does not support OpenID Connect Discovery.

#### System Preparation { .section}

Before you begin performing the steps of this activity, do the following:

1.  Deploy a new Acumatica ERP instance with the *T100* dataset. For details on deploying an instance, see [Instance Deployment: To Deploy an Instance with Demo Data](UserGuide/INST_Deploying_Instances_Deploy_Tenant_With_Demodata_Activity.md).
2.  To sign in to the instance in the client application, use the tenant name \(which you specified when you created the instance\) and the *MYSTORE* branch.
3.  Complete the following prerequisite activity: [Registration of an OAuth 2.0 or OIDC ApplicationActivity 1.1.1: To Register the Application in Acumatica ERP](#_3ab47faf-6e35-415d-88ba-f8b5a8f8c67e).

#### Step: Configuring a Postman Collection { .section}

To configure a Postman collection to use the OAuth 2.0 authorization in Acumatica ERP, do the following:

1.  If you use a self-signed certificate for HTTPS, in Postman settings, turn off SSL certificate verification.
2.  In Postman, create a collection.

    **Tip:** Instead of creating a collection, you can import to Postman the collection provided with this course \([`REST.postman_collection.json`](https://github.com/Acumatica/Help-and-Training-Examples/blob/HEAD/IntegrationDevelopment/I320/REST.postman_collection.json)\). This collection has been configured for this course and already contains all the requests that are used in the course. You can use this collection for testing the requests.

3.  On the **Authorization** tab of the collection properties window, which opens when the collection has been created, select the following values:
    -   **Auth Type**: *OAuth 2.0*
    -   **Add auth data to**: *Request Headers*
4.  In the **Configure New Token** section, specify the following values:
    -   **Token Name**: `MyStoreIntegration`
    -   **Grant Type**: *Password Credentials*
    -   **Access Token URL**: The token endpoint address, such as `https://localhost/MyStoreInstance/identity/connect/token`
    -   **Username**: `admin`
    -   **Password**: The password for the `admin` user
    -   **Client ID**: The client ID of the application, which you can copy from the **Client ID** box on the [Connected Applications](UserGuide/SM_30_30_10.md) \(SM303010\) form for the MyStoreIntegration client \(which you have created in [Registration of an OAuth 2.0 or OIDC ApplicationActivity 1.1.1: To Register the Application in Acumatica ERP](#_3ab47faf-6e35-415d-88ba-f8b5a8f8c67e)\)
    -   **Client Secret**: The client secret that you have received and saved during the registration of the MyStoreIntegration client on the [Connected Applications](UserGuide/SM_30_30_10.md) form
    -   **Scope**: `api`
    -   **Client Authentication**: *Send client credentials in body*
5.  Click **Get New Access Token**. Once the token is received, the **Manage Access Tokens** dialog box opens.

    **Tip:** In certain versions of Postman, the approach described in this section does not work. Instead of this approach, you can send a direct `POST` request to the token endpoint. In the body of the request, you should pass the client ID, the client secret, Acumatica ERP username and password, the type of the authorization flow, and the requested scope. For an example of this request, see the Postman `REST.postman_collection.json` collection provided with this course.For details about the parameters passed in the request body, see [Resource Owner Password Credentials Flow: Obtaining of an Access Token](#_822ddb44-b14f-481a-90c7-f15521e61702).

6.  In the **Manage Access Tokens** dialog box, click **Use Token**.

### Resource Owner Password Credentials Flow: Assessment Test Questions {#_b3c8ce73-8c1b-492a-b257-6540b1e603f2}

1.  Which access scope is enough to request from Acumatica ERP to obtain access to the web service APIs through OAuth 2.0?
    -   Only api
    -   Only offline\_access
    -   api and offline\_access
2.  Which response best describes the token or tokens you have to request from Acumatica ERP to obtain access to the web service APIs through OAuth 2.0?
    -   Only an access token
    -   Only a refresh token
    -   An access token and a refresh token
3.  Which endpoint should you use to retrieve the OAuth 2.0 access token from Acumatica ERP?
    -   The token endpoint
    -   The discovery endpoint
    -   Either the token endpoint or the discovery endpoint
4.  What information do you use to connect to Acumatica ERP using the OAuth 2.0 Resource Owner Password Credentials flow?
    -   The Acumatica ERP instance URL
    -   The client ID
    -   The Acumatica ERP username
    -   The Acumatica ERP user password
    -   The Acumatica ERP version

## Implementing the Hybrid Flow {#_254a70ba-bea9-4767-be6c-ee0ec9455692}

The Hybrid flow offers a balance between security and usability by leveraging the advantages of both the Authorization Code flow \(such as secure token exchange\) and the Implicit flow \(such as immediate access tokens\). In this chapter, you can find details about the implementation of the Hybrid flow.

### Hybrid Flow: General Information {#_f1bcf512-d676-4d8d-99d7-1a25fd565cdf}

When you implement OpenID Connect \(OIDC\) in a client application to make the application work with Acumatica ERP, you can use the Hybrid flow. This authorization flow is a combination of the Authorization Code flow and the Implicit flow. As with the Authorization Code flow, with the Hybrid flow, the client application requests the authorization code at the authorization endpoint. As with the Implicit flow, with the Hybrid flow, the client application requests the ID token, access token, or both at the token endpoint. The Hybrid flow allows an application to have immediate access to an ID token while providing secure retrieval of access and refresh tokens.

#### Learning Objectives { .section}

In this chapter, you will learn how to implement a client application that uses the Hybrid flow.

#### Applicable Scenarios { .section}

You implement the Hybrid flow in a client application that can securely store client secrets when the application needs to immediately access information about the user, but must perform some processing before gaining access to protected resources for a long period.

#### Hybrid Flow { .section}

For the support of the Hybrid flow, you implement the following general steps in the application:

1.  **Obtaining tokens from the authorization endpoint**

    The client application connects to the authorization endpoint of Acumatica ERP.

    The authorization endpoint directs the user of the client application to the sign-in page of Acumatica ERP, where the user should enter the credentials to sign in to a tenant configured in the Acumatica ERP instance.

    **Note:** The user must sign in to the tenant that was specified in the client\_id URL parameter passed to the authorization endpoint. \(This tenant is selected by default on the sign-in page.\)

    If the credentials are accepted by Acumatica ERP, the system displays the consent form, where the user can confirm that the application has access to the requested scopes. Only the scopes that were requested by the application are displayed on the consent form.

    If the user has successfully signed in to Acumatica ERP and has granted the access, a response is sent to the redirect URI specified in the authorization request. The response can contain an ID token, access token, and authorization code.

    If the ID token is retrieved, the client application validates it by using the key that is available on the [OpenID Connect Preferences](UserGuide/SM_30_30_30.md) \(SM303030\) form. The client application can obtain the key through a `GET` request to the following URL: *\[&lt;Acumatica ERP instance URL&gt;\]/identity/.well-known/openid-configuration/jwks*. The ID token contains the claims to which the user has granted access.

    For details on the requesting of tokens from the authorization endpoint, see [Hybrid Flow: Obtaining of an Authorization Code, Access Token, and ID Token from the Authorization Endpoint](#_9cd888ba-aa9a-4140-b5e9-4da297fade89).

2.  **Obtaining tokens from the token endpoint**

    If the client application uses JSON Web Token \(JWT\) bearer tokens, the application generates a JWT and signs it with the private key.

    The client application connects to the token endpoint of Acumatica ERP, submits the authorization code, and provides a signed JWT or a shared secret. If a JWT is provided, Acumatica ERP verifies the JWT signature by using the public key \(which was specified during the registration of the client application in Acumatica ERP\) and validates the JWT payload. If a shared secret is provided, Acumatica ERP verifies the provided application credentials.

    If verification is completed successfully, Acumatica ERP issues the access token, the ID token, and the refresh token if these tokens have been requested by the application. The client application should provide the access token with each data request to Acumatica ERP.

    If the ID token is retrieved, the client application validates it by using the key that is available on the [OpenID Connect Preferences](UserGuide/SM_30_30_30.md) \(SM303030\) form. The client application can obtain the key through a `GET` request to the following URL: *\[&lt;Acumatica ERP instance URL&gt;\]/identity/.well-known/openid-configuration/jwks*. The ID token contains the claims to which the user has granted access.

    For more information on this process, see [Hybrid Flow: Obtaining of an Access Token and ID Token from the Token Endpoint](#_275efb2f-4b16-4d08-9c43-728d5371848c).

3.  **Optional: Retrieving the user information**

    The client application requests user information from Acumatica ERP and provides the access token with this request. Acumatica ERP returns the information for which the user has provided the consent. For details about this request, see [OAuth 2.0 and OIDC: Obtaining of the User Data](#_065d2515-3e48-4ac6-8c62-56ae546f92eb).

    **Attention:** The recommended way of obtaining the user data is to parse the validated ID token, which contains the same claims as the ones that are obtained through this request.

4.  **Optional: Working with data in Acumatica ERP**

    The client application requests data from Acumatica ERP and provides the access token with this request. Acumatica ERP returns the requested data. For details on this process, see [OAuth 2.0 and OIDC: Working with Data in Acumatica ERP](#_ed9fe5ab-b3c0-444c-8335-ff3b0c8392d6).


When the access token expires, the client application can request a new access token by providing a refresh token, as described in [OAuth 2.0 and OIDC: Refreshing of an Access Token](#_2a5781c6-b661-409c-82f3-8adafd6a332d).

For details on the OAuth 2.0 authorization mechanism, see the specification at [https://tools.ietf.org/html/rfc6749](https://tools.ietf.org/html/rfc6749). For details on the OIDC authorization mechanism, see the specification at [https://openid.net/specs/openid-connect-core-1\_0.html\#Authentication](https://openid.net/specs/openid-connect-core-1_0.html#Authentication).

**Tip:** The configuration of the OpenID Connect protocol that is used by an Acumatica ERP website can be displayed by a request to the following URL: *\[&lt;Acumatica ERP instance URL&gt;\]/identity/.well-known/openid-configuration*. \(In this request, *\[&lt;Acumatica ERP instance URL&gt;\]* stands for the URL of the Acumatica ERP website.\)

#### Hybrid Flow Diagram { .section}

The following diagram illustrates the Hybrid flow.

![](IntegrationDevelopmentGuide/Images/diag_HybridFlow.png "Hybrid flow")

### Hybrid Flow: Obtaining of an Authorization Code, Access Token, and ID Token from the Authorization Endpoint {#_9cd888ba-aa9a-4140-b5e9-4da297fade89}

To obtain an authorization code, ID token, and access token from the authorization endpoint, the client application connects to the authorization endpoint of Acumatica ERP with the `GET` HTTP method and specifies the parameters of the request in the URL. For details on the request and the response, see the following sections.

#### HTTP Method and URL { .section}

The client application connects to the authorization endpoint of Acumatica ERP with the `GET` method. The client application can use one of the following approaches for the URL:

-   If the client application supports OpenID Connect Discovery, the client application can use the discovery endpoint address, as shown in the following code.

    ```
    https://<Acumatica ERP instance URL>/identity/.well-known/openid-configuration
    ```

    **Tip:** We recommend that the client application use the discovery endpoint address to obtain the token endpoint address. The use of the discovery endpoint eliminates the need to change the application if the address of the token endpoint changes.

    **Attention:** A request to the discovery endpoint does not provide the access token; it provides the address of the token endpoint from which you can receive the access token.

-   The client application can directly use the authorization endpoint address, which is shown in the following code.

    ```
    https://<Acumatica ERP instance URL>/identity/connect/authorize
    ```


#### Parameters { .section}

The client application should specify the following URL parameters.

<table id="_eac87a00-7bf2-4d20-90c7-8d4a29eab2ae"><thead><tr><th>

Parameter

</th><th>

Description

</th></tr></thead><tbody><tr><td>

response\_type

</td><td>

The type of the response, which can be one of the following:-   `code id_token`: Is used to retrieve an ID token and authorization code.
-   `code token`: Is used to retrieve an access token and authorization code. No ID token is returned.
-   `code id_token token`: Is used to retrieve an ID token, access token, and authorization code.

</td></tr><tr><td>

client\_id

</td><td>

The client ID that was assigned to the client application during the registration of the application in Acumatica ERP. The client ID must have the format in which the ID was generated during the registration of the application. That is, the client ID must include an auto-generated string and the ID of the tenant, such as *88358B02-A48D-A50E-F710-39C1636C30F6@MyTenant*. The client application will have access to the data of the tenant specified in the client ID.

</td></tr><tr><td>

redirect\_uri

</td><td>

The URI in the client application to which the response to the request should be sent. The URI must exactly match one of the values specified for the application in the **Redirect URI** column on the **Redirect URIs** tab of the [Connected Applications](UserGuide/SM_30_30_10.md#) \(SM303010\) form.

</td></tr><tr><td>

response\_mode

</td><td>

The way the system sends the request to the redirect URI in response for the authorization request. The response mode can be one of the following: -   `form_post`: The system uses the `POST` HTTP method to send a request to the redirect\_uri address. The request body, which includes the response parameters, has `application/x-www-form-urlencoded` format.
-   `fragment`: The system redirects the client application to the redirect\_uri address and adds all response parameters to the fragment component of the redirect URI.

</td></tr><tr><td>

scope

</td><td>

The access scope that is requested by the client application. The scope can be a combination of the following values, delimited by spaces:

 -   openid: Requests access to the personal information of the user. This scope is mandatory for the Hybrid flow.
-   email: Requests disclosure of the user's email address.
-   profile: Requests disclosure of the user's profile information.
-   phone: Requests disclosure of the user's phone number.
-   api: Requests access to the REST API, screen-based SOAP API, and OData interface.

If this scope is granted and the api:concurrent\_access scope is not granted, Acumatica ERP manages the sessions of the application through tokens. Acumatica ERP issues the first access token along with the session ID. If the client application requests a new access token by presenting a refresh token, Acumatica ERP reuses the session ID that was issued for the first access token issued with the refresh token. That is, the system uses a single session for each access granted to the client application. For details about the license limitations related to the number of sessions for client applications, see [License Restrictions for API Users](IntegrationDevelopmentGuide/IS__con_License_Restrictions_API_Users.md).

-   offline\_access: Requests that a refresh token be granted. If a user grants this scope to the application, Acumatica ERP issues to the client application a refresh token along with the access token. When the access token has expired, the client application can request a new access token by sending a request to the token endpoint and providing the refresh token. By default, the whole chain for the refresh token expires 30 days after the initial authentication process. However, you can change these settings in the **Refresh Tokens** section of the Summary area of the [Connected Applications](UserGuide/SM_30_30_10.md) \(SM303010\) form. For details, see [Registration of an OAuth 2.0 or OIDC Application: Sliding Expiration of Refresh Tokens](#_92bf610c-f18c-446c-8e62-5fb928ef2def).
-   api:concurrent\_access: Requests permission for the concurrent use of multiple types of web service APIs. If a user grants this scope to the application, the client application can access data in Acumatica ERP in concurrent mode. In this case, Acumatica ERP can maintain multiple sessions for the client application, managing session IDs through cookies. We recommend that the client application request this scope only if concurrent access is required for the client application. For details about the license limitations related to the number of sessions for client applications, see [License Restrictions for API Users](IntegrationDevelopmentGuide/IS__con_License_Restrictions_API_Users.md).

</td></tr><tr><td>

nonce

</td><td>

A string value that is used to associate a client session with an ID token.

</td></tr></tbody>
</table>#### Response { .section}

If the user is successfully signed in to Acumatica ERP and has granted access, a response is sent to the redirect URI specified in the authorization request. The `response_mode` parameter of the authorization request defines the way the request is sent. The response includes the following parameters.

**Tip:** The refresh token is not returned from the authorization endpoint. To obtain the refresh token, you need to send a request to the token endpoint with the received authorization code, as described in [Hybrid Flow: Obtaining of an Access Token and ID Token from the Token Endpoint](#_275efb2f-4b16-4d08-9c43-728d5371848c).

<table id="_dc2096a3-af1b-49f8-a580-f26674cc082d"><thead><tr><th>

Parameter

</th><th>

Description

</th></tr></thead><tbody><tr><td>

code

</td><td>

The authorization code.

</td></tr><tr><td>

id\_token

</td><td>

The ID token associated with the authenticated session. The ID token contains three parts, which are separated by periods. The parts are Base64 encoded. The second part contains the claims to which the user granted access. For details on the ID token structure, see [https://openid.net/specs/openid-connect-core-1\_0.html\#IDToken](https://openid.net/specs/openid-connect-core-1_0.html#IDToken) and [https://www.rfc-editor.org/rfc/rfc7519.html](https://www.rfc-editor.org/rfc/rfc7519.html). We recommend that you use the existing standard libraries for parsing the tokens. The parameter is returned only if the openid scope was granted.

</td></tr><tr><td>

scope

</td><td>

The scope for which the access token and ID token are provided. The returning of this parameter is optional.

</td></tr><tr><td>

access\_token

</td><td>

The access token. The parameter is returned only if the api scope was granted.

</td></tr><tr><td>

token\_type

</td><td>

The type of the access token, which is *Bearer*. The parameter is returned only if the api scope was granted.

</td></tr><tr><td>

expires\_in

</td><td>

The period of time \(in seconds\) during which the access token is valid. The parameter is returned only if the api scope was granted.

</td></tr></tbody>
</table>#### Example: openid and email Scopes { .section}

The following example requests the *openid* and *email* scopes. \(Line breaks are for display purposes only.\)

```
GET https://localhost/AcumaticaDB/identity/connect/authorize?
response_type=code id_token
&client_id=58FCCFBD-0CF3-C047-B720-A631C976A8DD@U100
&redirect_uri=https://localhost
&scope=openid email
&response_mode=fragment
&nonce=test
```

Once the user grants access to the requested scopes, Acumatica ERP redirects the client application to the following URL.

```
https://localhost/#
code=fXatQXiNwxDc3YSy7Agjz_fKAJBUVN2UmpqTMLtVidY
&id_token=eyJ...gzw
&scope=openid%20email
```

#### Example: openid, email, profile, and api Scopes { .section}

The following example requests the *openid*, *email*, *profile*, and *api* scopes. \(Line breaks are for display purposes only.\)

```
GET https://localhost/AcumaticaDB/identity/connect/authorize?
response_type=code id_token token
&client_id=58FCCFBD-0CF3-C047-B720-A631C976A8DD@U100
&redirect_uri=https://localhost
&scope=openid email profile api
&response_mode=fragment
&nonce=test
```

Once the user grants access to the requested scopes, Acumatica ERP redirects the client application to the following URL.

```
https://localhost/#
code=Xa8dL8wAL23PmZEdoCBzTDJyj46_NPx_pplzlf-tFas
&id_token=eyJ...EMo
&token_type=Bearer
&expires_in=3600
&scope=openid%20email%20profile%20api
```

### Hybrid Flow: Obtaining of an Access Token and ID Token from the Token Endpoint {#_275efb2f-4b16-4d08-9c43-728d5371848c}

To obtain an ID token and access token from the token endpoint, a client application that implements the Hybrid flow connects to the token endpoint of Acumatica ERP with the `POST` method. For details on the request and the response, see the following sections.

#### HTTP Method and URL { .section}

The client application connects to the token endpoint of Acumatica ERP with the `POST` method. The client application can use one of the following options for the URL:

-   If the client application supports OpenID Connect Discovery, the client application can use the discovery endpoint address, as shown in the following code.

    ```
    https://<Acumatica ERP instance URL>/identity/.well-known/openid-configuration
    ```

    **Tip:** We recommend that the client application use the discovery endpoint address to obtain the token endpoint address. The use of the discovery endpoint eliminates the need to change the application if the address of the token endpoint changes.

    **Attention:** A request to the discovery endpoint does not provide the access token; it provides the address of the token endpoint from which you can receive the access token.

-   The client application can directly use the token endpoint address, as shown in the following code.

    ```
    https://<Acumatica ERP instance URL>/identity/connect/token
    ```


#### HTTP Header { .section}

You use the following HTTP header.

|Key|Value|
|---|-----|
|`Content-Type`|`application/x-www-form-urlencoded`|

#### Request Body { .section}

You specify the following parameters in the request body.

<table id="_3b0e0bbc-beae-4cfb-8cc9-eb317d556603"><thead><tr><th>

Parameter

</th><th>

Description

</th></tr></thead><tbody><tr><td>

grant\_type

</td><td>

The type of the flow, which must be set to *authorization\_code* for the Hybrid flow.

</td></tr><tr><td>

client\_id

</td><td>

The client ID that was assigned to the client application during the registration of the application in Acumatica ERP. The client ID must have the format in which the ID was generated during the registration of the application. That is, the client ID must include an auto-generated string and the ID of the tenant, such as *88358B02-A48D-A50E-F710-39C1636C30F6@MyTenant*. The client application will have access to the data of the tenant specified in the client ID.

</td></tr><tr><td>

code

</td><td>

The authorization code that the client application has received from the authorization endpoint.

</td></tr><tr><td>

client\_secret

</td><td>

For a client application that uses a shared secret, the value of the secret that was created for the client application during the registration of the application in Acumatica ERP.

</td></tr><tr><td>

client\_assertion\_type

</td><td>

For a client application that uses JSON Web Token \(JWT\) bearer tokens, the client assertion type, which must be set to *urn:ietf:params:oauth:client-assertion-type:jwt-bearer*.

</td></tr><tr><td>

client\_assertion

</td><td>

For a client application that uses JSON Web Token \(JWT\) bearer tokens, a single JWT.

</td></tr><tr><td>

redirect\_uri

</td><td>

The URI in the client application to which the response to the request should be sent. The URI must exactly match one of the values specified for the application in the **Redirect URI** column on the **Redirect URIs** tab of the [Connected Applications](UserGuide/SM_30_30_10.md#) \(SM303010\) form.

</td></tr></tbody>
</table>#### Response { .section}

Acumatica ERP verifies the provided application credentials and issues an access token, an ID token, and a refresh token if they have been requested by the application. The client application should provide the access token with each data request to Acumatica ERP.

A successful response includes the following parameters in the response body.

<table id="_dc2096a3-af1b-49f8-a580-f26674cc082d"><thead><tr><th>

Parameter

</th><th>

Description

</th></tr></thead><tbody><tr><td>

token\_type

</td><td>

The type of the access token, which is *Bearer*. The parameter is returned only if the api scope was granted.

</td></tr><tr><td>

access\_token

</td><td>

The access token. The parameter is returned only if the api scope was granted.

</td></tr><tr><td>

expires\_in

</td><td>

The period of time \(in seconds\) during which the access token is valid. The parameter is returned only if the api scope was granted.

</td></tr><tr><td>

scope

</td><td>

The scope for which the access token and ID token are provided. The returning of this parameter is optional.

</td></tr><tr><td>

refresh\_token

</td><td>

The refresh token. The parameter is returned only if the offline\_access scope was granted.

</td></tr><tr><td>

id\_token

</td><td>

The ID token associated with the authenticated session. The ID token contains three parts, which are separated by periods. The parts are Base64 encoded. The second part contains the claims to which the user granted access. For details on the ID token structure, see [https://openid.net/specs/openid-connect-core-1\_0.html\#IDToken](https://openid.net/specs/openid-connect-core-1_0.html#IDToken) and [https://www.rfc-editor.org/rfc/rfc7519.html](https://www.rfc-editor.org/rfc/rfc7519.html). We recommend that you use the existing standard libraries for parsing the tokens. The parameter is returned only if the openid scope was granted.

</td></tr></tbody>
</table>#### Example { .section}

The following example shows a request for access token with a shared secret provided with the request. \(Line breaks are for display purposes only.\)

```
POST /identity/connect/token HTTP/1.1
Host: https://localhost/AcumaticaDB
Content-Type: application/x-www-form-urlencoded

grant_type=authorization_code
&client_id=C07F7B7A-8947-3C56-2B27-A46CA1F8EF8F@U100
&client_secret=sJli5nNartFBX4Ckzpb68g
&code=z0ExIPH9pAdJSc5nDVakHwYW2jnt91B9oyoZQvdp3cQ
&scope=openid%20email%20profile%20api%20offline_access
&redirect_uri=https://localhost
```

A successful response has the body shown in the following example.

```
{
    "id_token": "eyJ...Z5A",
    "access_token": "9zx2acU03l0ORLHfqwdCPxFHWJMzlLDqrOfJjlZcb_I",
    "expires_in": 3600,
    "token_type": "Bearer",
    "refresh_token": "sbUAHaA7xST3vnvlsanRh3M5EWNmAW_fu6CX16ZEQqM",
    "scope": "openid email profile api offline_access"
}
```


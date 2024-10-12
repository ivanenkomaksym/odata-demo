# odata-demo

This project covers a set of OData features in ASP.NET applications:
 * Building EDM model
 * Custom resource serializer

By using feature flags in **appsettings.json** we can define OData resource serializer behavior:
 * **OmitNull** - completely omit all null values from the OData response
 * **ClientOmitNull** - same as previous one, but controlled by the client request header **Prefer: omit-values=nulls**
 * **OmitDefaultValue** - omit defiend properties with default value from the OData response
 * **FilterQueryOptionRequired** - force the client to pass $filter clause

# Example

## Client controlled omit nulls

Set **"ClientOmitNull": true** in the **appsettings.json** and in **Postman** send the following request with additional request header:
```
GET http://localhost:5220/odata/Customers
Prefer: omit-values=nulls
```

Response should not contain properties with **null** value, such as **UserRole**:

```
{
    "@odata.context": "http://localhost:5220/odata/$metadata#Customers",
    "value": [
        {
            "id": 1,
            "name": "Customer 1",
            "stringPropertyWithDefaultValueToBeOmitted": "",
            "booleanPropertyWithDefaultValueToBeOmitted": false
        },
```

## Omit default value

Set **"OmitDefaultValue": true** in the **appsettings.json** and send the following request:
```
http://localhost:5220/odata/Customers
```

Response should not contain neither **StringPropertyWithDefaultValueToBeOmitted**, nor **BooleanPropertyWithDefaultValueToBeOmitted**, which where configured to be skipped if contain default values:
```
{
  "@odata.context": "http://localhost:5220/odata/$metadata#Customers",
  "value": [
    {
      "id": 1,
      "name": "Customer 1",
      "userRole": null
    },
```

# References
[Extension: Omit null value properties in ASP.NET Core OData](https://devblogs.microsoft.com/odata/extension-omit-null-value-properties-in-asp-net-core-odata/)

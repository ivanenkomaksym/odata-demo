using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.OData.Edm;
using Microsoft.OData;

namespace ODataDemo.Serializers
{
    /// <summary>
    /// Omits null value from OData response.
    /// Inspired by https://devblogs.microsoft.com/odata/extension-omit-null-value-properties-in-asp-net-core-odata/
    /// </summary>
    public class OmitNullResourceSerializer : ODataResourceSerializer
    {
        public OmitNullResourceSerializer(IODataSerializerProvider serializerProvider) : base(serializerProvider)
        {
        }

        public override ODataProperty CreateStructuralProperty(IEdmStructuralProperty structuralProperty, ResourceContext resourceContext)
        {
            object propertyValue = resourceContext.GetPropertyValue(structuralProperty.Name);
            if (propertyValue == null)
            {
                return null;
            }

            return base.CreateStructuralProperty(structuralProperty, resourceContext);
        }
    }
}

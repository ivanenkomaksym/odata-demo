using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.OData;
using Microsoft.OData.Edm;
using ODataDemo.Extensions;

namespace ODataDemo.Serializers
{
    public class ClientOmitNullResourceSerializer : ODataResourceSerializer
    {
        public ClientOmitNullResourceSerializer(IODataSerializerProvider serializerProvider) : base(serializerProvider)
        {
        }

        public override ODataProperty CreateStructuralProperty(IEdmStructuralProperty structuralProperty, ResourceContext resourceContext)
        {
            bool isOmitNulls = resourceContext.Request.ShouldOmitNullValues();
            if (isOmitNulls)
            {
                object propertyValue = resourceContext.GetPropertyValue(structuralProperty.Name);
                if (propertyValue == null)
                {
                    return null;
                }
            }

            return base.CreateStructuralProperty(structuralProperty, resourceContext);
        }
    }
}

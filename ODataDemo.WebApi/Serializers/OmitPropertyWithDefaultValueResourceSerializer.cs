using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.OData;
using Microsoft.OData.Edm;

namespace ODataDemo.Serializers
{
    /// <summary>
    /// Omits property with default value.
    /// </summary>
    public class OmitPropertyWithDefaultValueResourceSerializer : ODataResourceSerializer
    {
        public OmitPropertyWithDefaultValueResourceSerializer(IODataSerializerProvider serializerProvider,
                                                              Dictionary<string, object> propertyNameToDefaultValueRegistry)
            : base(serializerProvider)
        {
            PropertyNameToDefaultValueRegistry = propertyNameToDefaultValueRegistry;
        }

        public override ODataProperty CreateStructuralProperty(IEdmStructuralProperty structuralProperty, ResourceContext resourceContext)
        {
            object propertyValue = resourceContext.GetPropertyValue(structuralProperty.Name);
            if (PropertyNameToDefaultValueRegistry.TryGetValue(structuralProperty.Name, out var defaultValue) &&
                propertyValue.Equals(defaultValue))
            {
                return null;
            }

            return base.CreateStructuralProperty(structuralProperty, resourceContext);
        }

        private readonly Dictionary<string, object> PropertyNameToDefaultValueRegistry;
    }
}

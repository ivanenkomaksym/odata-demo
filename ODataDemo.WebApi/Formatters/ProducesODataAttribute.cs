using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace ODataDemo.Formatters
{
    public class ProducesODataAttribute : ProducesAttribute
    {
        public ProducesODataAttribute(IHostEnvironment environment)
            : base(MediaTypeNames.Application.Json, GetAcceptedTypes(environment))
        {
        }

        private static string[] GetAcceptedTypes(IHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                return [ODataMediaTypes.ApplicationJsonODataFullMetadata,
                        ODataMediaTypes.ApplicationJsonODataMinimalMetadata,
                        ODataMediaTypes.ApplicationJsonODataNoMetadata];
            }

            return [ODataMediaTypes.ApplicationJsonODataNoMetadata];
        }
    }
}

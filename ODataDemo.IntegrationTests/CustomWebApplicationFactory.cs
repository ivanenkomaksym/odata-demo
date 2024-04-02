using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration.Memory;

namespace ODataDemo.IntegrationTests
{
    internal class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly string FeatureFlag;

        public CustomWebApplicationFactory(string featureFlag)
        {
            FeatureFlag = featureFlag;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(configurationBuilder =>
            {
                configurationBuilder.Sources.Clear();

                var configuration = new[] { KeyValuePair.Create<string, string?>($"FeatureManagement:{FeatureFlag}", "true") };
                var memoryConfiguration = new MemoryConfigurationSource { InitialData = configuration };

                configurationBuilder.Add(memoryConfiguration);
            });
        }
    }
}

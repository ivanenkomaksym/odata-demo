using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using ODataDemo.Models;
using ODataDemo.Repository;

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

            builder.ConfigureServices(services =>
            {
                var customerRepositoryMock = Substitute.For<ICustomerRepository>();
                customerRepositoryMock.GetCustomers().Returns(GetMockCustomers());

                // Replace the ICustomerRepository registration with the mock
                services.Replace(new ServiceDescriptor(typeof(ICustomerRepository), customerRepositoryMock));
            });
        }

        private static IQueryable<Customer> GetMockCustomers()
        {
            var mockCustomers = new List<Customer>
            {
                new() 
                { 
                    Id = 1,
                    Name = "Mock Customer 1",
                    UserRole = null,
                },
                new()
                {
                    Id = 2,
                    Name = "Mock Customer 2",
                    UserRole = null,
                }
            };

            return mockCustomers.AsQueryable();
        }
    }
}

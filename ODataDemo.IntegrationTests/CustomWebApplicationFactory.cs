using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using ODataDemo.Repository;

namespace ODataDemo.IntegrationTests
{
    internal class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private readonly string FeatureFlag;
        private readonly ICustomerRepository CustomerRepository;

        public CustomWebApplicationFactory(string featureFlag)
        {
            FeatureFlag = featureFlag;
            CustomerRepository = new TestCustomerRepository();
        }

        public CustomWebApplicationFactory(string featureFlag, ICustomerRepository customerRepository)
        {
            FeatureFlag = featureFlag;
            CustomerRepository = customerRepository;
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
                customerRepositoryMock.GetCustomers().Returns(CustomerRepository.GetCustomers());

                // Replace the ICustomerRepository registration with the mock
                services.Replace(new ServiceDescriptor(typeof(ICustomerRepository), customerRepositoryMock));
            });
        }
    }
}

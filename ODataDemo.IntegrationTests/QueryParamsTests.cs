using ODataDemo.Extensions;
using ODataDemo.Models;
using ODataDemo.Repository;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace ODataDemo.IntegrationTests
{
    public class QueryParamsTests
    {
        private readonly JsonSerializerOptions JsonSerializerOptions;

        public QueryParamsTests()
        {
            JsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        }

        [Fact]
        public async Task FilterClauseShouldFilter()
        {
            // Arrange
            var ids = new List<string> { "Customer 1", "Customer 3" };
            var factory = new CustomWebApplicationFactory(FeatureFlags.FilterQueryOptionRequired, new StaticCustomerRepository());
            var client = factory.CreateClient();
            var url = $"odata/Customers?$filter=Name in ('{ids[0]}', '{ids[1]}')";

            // Act
            var response = await client.GetAsync(url);
            var jsonString = await response.Content.ReadAsStringAsync();
            var jsonNode = JsonNode.Parse(jsonString);
            var customersJson = jsonNode?["value"]?.ToJsonString();
            
            var customers = JsonSerializer.Deserialize<IEnumerable<Customer>>(customersJson, JsonSerializerOptions);

            // Assert
            Assert.NotNull(customers);
            Assert.Equal(2, customers.Count());
            var matchedCustomers = customers.Where(c => ids.Contains(c.Name));
            Assert.Equal(2, matchedCustomers.Count());
        }

        [Fact]
        public async Task SelectClauseShouldSelect()
        {
            // Arrange
            var ids = new List<string> { "Customer 1", "Customer 3" };
            var factory = new CustomWebApplicationFactory(FeatureFlags.FilterQueryOptionRequired, new StaticCustomerRepository());
            var client = factory.CreateClient();
            var url = $"odata/Customers?$filter=Name in ('{ids[0]}', '{ids[1]}')&$select=id,name";

            // Act
            var response = await client.GetAsync(url);
            var responseAsStr = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains(nameof(Customer.Id).ToCamelCase(), responseAsStr);
            Assert.Contains(nameof(Customer.Name).ToCamelCase(), responseAsStr);
            Assert.DoesNotContain(nameof(Customer.UserRole).ToCamelCase(), responseAsStr);
            Assert.DoesNotContain(nameof(Customer.Orders).ToCamelCase(), responseAsStr);
            Assert.DoesNotContain(nameof(Customer.StringPropertyWithDefaultValueToBeOmitted).ToCamelCase(), responseAsStr);
            Assert.DoesNotContain(nameof(Customer.BooleanPropertyWithDefaultValueToBeOmitted).ToCamelCase(), responseAsStr);
            Assert.DoesNotContain(nameof(Customer.Contract).ToCamelCase(), responseAsStr);
        }

        [Fact]
        public async Task ExpandClauseShouldExpand()
        {
            // Arrange
            var ids = new List<string> { "Customer 1", "Customer 3" };
            var factory = new CustomWebApplicationFactory(FeatureFlags.FilterQueryOptionRequired, new StaticCustomerRepository());
            var client = factory.CreateClient();
            var urlWithExpandContract = $"odata/Customers?$filter=Name in ('{ids[0]}', '{ids[1]}')&$expand=contract";
            var urlWithExpandContractAndAddress = $"odata/Customers?$filter=Name in ('{ids[0]}', '{ids[1]}')&$expand=contract($expand=address)";

            // Act
            var responseWithExpandContract = await client.GetAsync(urlWithExpandContract);
            var responseWithExpandContractAsStr = await responseWithExpandContract.Content.ReadAsStringAsync();

            var responseWithExpandContractAndAddress = await client.GetAsync(urlWithExpandContractAndAddress);
            var responseWithExpandContractAndAddressAsStr = await responseWithExpandContractAndAddress.Content.ReadAsStringAsync();

            // Assert
            responseWithExpandContract.EnsureSuccessStatusCode();
            Assert.Contains(nameof(Customer.Contract).ToCamelCase(), responseWithExpandContractAsStr);
            Assert.DoesNotContain(nameof(IContract.Address).ToCamelCase(), responseWithExpandContractAsStr);

            responseWithExpandContractAndAddress.EnsureSuccessStatusCode();
            Assert.Contains(nameof(IContract.Address).ToCamelCase(), responseWithExpandContractAndAddressAsStr);
        }
    }
}

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
    }
}

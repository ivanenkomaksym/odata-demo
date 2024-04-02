using ODataDemo.Models;

namespace ODataDemo.IntegrationTests
{
    public class ResourceSerializerTests
    {
        [Fact]
        public async Task OmitNull()
        {
            // Arrange
            var factory = new CustomWebApplicationFactory(FeatureFlags.OmitNull);
            var client = factory.CreateClient();
            var url = "odata/Customers";

            // Act
            var response = await client.GetAsync(url);
            var responseAsStr = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.DoesNotContain(nameof(Customer.UserRole), responseAsStr);
        }

        [Fact]
        public async Task OmitDefaultValue()
        {
            // Arrange
            var factory = new CustomWebApplicationFactory(FeatureFlags.OmitDefaultValue);
            var client = factory.CreateClient();
            var url = "odata/Customers";

            // Act
            var response = await client.GetAsync(url);
            var responseAsStr = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.DoesNotContain(nameof(Customer.StringPropertyWithDefaultValueToBeOmitted), responseAsStr);
            Assert.DoesNotContain(nameof(Customer.BooleanPropertyWithDefaultValueToBeOmitted), responseAsStr);
        }
    }
}
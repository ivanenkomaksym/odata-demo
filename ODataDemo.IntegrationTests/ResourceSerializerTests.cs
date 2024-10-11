using ODataDemo.Extensions;
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
            Assert.DoesNotContain(nameof(Customer.UserRole).ToCamelCase(), responseAsStr);
        }

        [Fact]
        public async Task ClientOmitNull()
        {
            // Arrange
            var factory = new CustomWebApplicationFactory(FeatureFlags.ClientOmitNull);
            var client = factory.CreateClient();
            var url = "odata/Customers";

            // Act
            var responseWithoutHeader = await client.GetAsync(url);
            var responseWithoutHeaderAsStr = await responseWithoutHeader.Content.ReadAsStringAsync();

            client.DefaultRequestHeaders.Add(HttpRequestExtensions.PreferHeaderName, HttpRequestExtensions.OmitNullValuesHeaderValue);
            var responseWithHeader = await client.GetAsync(url);
            var responseWithHeaderAsStr = await responseWithHeader.Content.ReadAsStringAsync();

            // Assert
            responseWithoutHeader.EnsureSuccessStatusCode();
            Assert.Contains(nameof(Customer.UserRole).ToCamelCase(), responseWithoutHeaderAsStr);

            responseWithHeader.EnsureSuccessStatusCode();
            Assert.DoesNotContain(nameof(Customer.UserRole).ToCamelCase(), responseWithHeaderAsStr);
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
            Assert.DoesNotContain(nameof(Customer.StringPropertyWithDefaultValueToBeOmitted).ToCamelCase(), responseAsStr);
            Assert.DoesNotContain(nameof(Customer.BooleanPropertyWithDefaultValueToBeOmitted).ToCamelCase(), responseAsStr);
        }

        [Fact]
        public async Task MissingFilterQueryOptionMustFail()
        {
            // Arrange
            var factory = new CustomWebApplicationFactory(FeatureFlags.FilterQueryOptionRequired);
            var client = factory.CreateClient();
            var url = "odata/Customers";

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact(Skip = "TODO: Check how it would fail")]
        public async Task KeySegmentTemplateTryTranslateThrowsUnhandleableException()
        {
            // Arrange
            var factory = new CustomWebApplicationFactory(FeatureFlags.OmitNull);
            var client = factory.CreateClient();
            var url = "odata/Customers/abc'";

            // Act
            var response = await client.GetAsync(url);
            var responseAsStr = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
            Assert.Contains("The key value ($filter=status eq 'New') from request is not valid. The key value should be format of type 'Edm.String'", responseAsStr);
        }
    }
}
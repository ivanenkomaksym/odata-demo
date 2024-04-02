namespace ODataDemo.IntegrationTests
{
    public class ResourceSerializerTests
    {
        [Fact]
        public async Task GetWeatherForecat()
        {
            // Arrange
            var factory = new CustomWebApplicationFactory(FeatureFlags.OmitNull);
            var client = factory.CreateClient();
            var url = "odata/Customers";

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }
    }
}
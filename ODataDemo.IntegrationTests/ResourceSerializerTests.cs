using Microsoft.AspNetCore.Mvc.Testing;

namespace ODataDemo.IntegrationTests
{
    public class ResourceSerializerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ResourceSerializerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetWeatherForecat()
        {
            // Arrange
            var url = "weatherforecast";

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }
    }
}
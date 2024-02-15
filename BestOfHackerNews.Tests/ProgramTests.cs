using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.TestHost;

[TestClass]
public class BestStoriesEndpointTests
{
    private static WebApplicationFactory<Program> _factory;

    [ClassInitialize]
    public static void ClassInitialize(TestContext _)
    {
        _factory = new WebApplicationFactory<Program>();
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        _factory.Dispose();
    }

    [TestMethod]
    public async Task GetBestStories_ReturnsSuccess()
    {
        // Arrange
        var key = Guid.NewGuid().ToString();
        Environment.SetEnvironmentVariable("ApiKeys:" + key, "test key");

        using var client = _factory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Get, "/beststories");
        request.Headers.Add("X-Api-Key", key);

        // Act
        var response = await client.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("Success!");
    }
}
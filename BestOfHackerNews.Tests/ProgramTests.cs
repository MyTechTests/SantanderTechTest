using FluentAssertions;
using Flurl.Http.Testing;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace BestOfHackerNews.Tests;

[TestClass]
public class BestStoriesEndpointTests
{
    private static WebApplicationFactory<Program> _factory = null!;

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
    public async Task GetBestStories_When_Called_Immediately_On_Startup_Returns_No_Content()
    {
        // Arrange
        var key = Guid.NewGuid().ToString();
        Environment.SetEnvironmentVariable("ApiKeys:" + key, "test key");

        var httpTest = new HttpTest();
        httpTest.ForCallsTo("https://example.com/best-stories").RespondWith("[]");

        using var client = _factory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Get, "/bestnstories/5");
        request.Headers.Add("X-Api-Key", key);

        // Act
        var response = await client.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [TestMethod]
    public async Task AddCommonRateLimiter_Should_Return_429_TooManyRequests_When_Exceeding_Rate_Limit()
    {
        // Arrange
        var key = Guid.NewGuid().ToString();
        Environment.SetEnvironmentVariable("ApiKeys:" + key, "test key");
        using var client = _factory.CreateClient();

        // Act
        var response1 = await SendRequest(key, client);
        var response2 = await SendRequest(key, client);
        var response3 = await SendRequest(key, client);
        var response4 = await SendRequest(key, client);
        var response5 = await SendRequest(key, client);
        var response6 = await SendRequest(key, client);

        // Assert
        response1.StatusCode = HttpStatusCode.OK;
        response2.StatusCode = HttpStatusCode.OK;
        response3.StatusCode = HttpStatusCode.OK;
        response4.StatusCode = HttpStatusCode.OK;
        response5.StatusCode = HttpStatusCode.OK;
        response6.StatusCode = HttpStatusCode.TooManyRequests;
        (await response6.Content.ReadAsStringAsync()).Should().Be("Too many requests. Please try later again... ");
    }

    private static Task<HttpResponseMessage> SendRequest(string key, HttpClient client)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/bestnstories/5");
        request.Headers.Add("X-Api-Key", key);
        return client.SendAsync(request);
    }
}
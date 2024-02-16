using BestOfHackerNews.Core.Extensions;
using BestOfHackerNews.Core.Interfaces;
using BestOfHackerNews.Core.Records;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Serilog;

namespace BestOfHackerNews.Core.Tests.Extensions;


[TestClass]
public class WebApplicationExtensionsTests
{
    [TestMethod]
    public void Handler_Should_Return_NotAcceptable_If_Count_Is_Zero()
    {
        // Arrange
        const int count = 0;
        var response = Substitute.For<HttpResponse>();
        var context = new DefaultHttpContext();
        var bestStoriesProvider = Substitute.For<IProvideBestStories>();
        const string expectedDetail = "The number of stories requested must be at least one.";

        context.RequestServices = new ServiceCollection()
            .AddSingleton(bestStoriesProvider)
            .BuildServiceProvider();

        // Act
        var result = WebApplicationExtensions.BohnGetHandler(count, response, context) as ProblemHttpResult;

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status406NotAcceptable);
        result.ProblemDetails.Detail.Should().Be(expectedDetail);
    }

    [TestMethod]
    public void Handler_Should_Return_Ok_With_Result_If_Count_Is_Positive_And_Stories_Present()
    {
        // Arrange
        const int count = 5;
        var response = Substitute.For<HttpResponse>();
        var context = new DefaultHttpContext();
        var bestStoriesProvider = Substitute.For<IProvideBestStories>();
        var expectedResult = new[] { new Story() };

        bestStoriesProvider.GetBestStories(count).Returns(expectedResult);

        context.RequestServices = new ServiceCollection()
            .AddSingleton(bestStoriesProvider)
            .BuildServiceProvider();

        // Act
        var result = WebApplicationExtensions.BohnGetHandler(count, response, context) as IStatusCodeHttpResult;

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [TestMethod]
    public void Handler_Should_Return_No_Content_With_Result_If_Count_Is_Positive_And_No_Stories_Present()
    {
        // Arrange
        const int count = 5;
        var response = Substitute.For<HttpResponse>();
        var context = new DefaultHttpContext();
        var bestStoriesProvider = Substitute.For<IProvideBestStories>();
        var expectedResult = Array.Empty<Story>();

        bestStoriesProvider.GetBestStories(count).Returns(expectedResult);

        context.RequestServices = new ServiceCollection()
            .AddSingleton(bestStoriesProvider)
            .BuildServiceProvider();

        // Act
        var result = WebApplicationExtensions.BohnGetHandler(count, response, context) as IStatusCodeHttpResult;

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }

    [TestMethod]
    public void Handler_Should_Return_InternalServerError_If_Exception_Occurs()
    {
        // Arrange
        const int count = 5;
        var response = Substitute.For<HttpResponse>();
        var context = new DefaultHttpContext();
        var bestStoriesProvider = Substitute.For<IProvideBestStories>();
        var someErrorMessage = "Some error message";
        var expectedException = new Exception(someErrorMessage);
        var logger = Substitute.For<ILogger>();

        bestStoriesProvider
            .When(x => x.GetBestStories(count))
            .Do(x => throw expectedException);

        context.RequestServices = new ServiceCollection()
            .AddSingleton(bestStoriesProvider)
            .AddSingleton(logger)
            .BuildServiceProvider();

        // Act
        var result = WebApplicationExtensions.BohnGetHandler(count, response, context) as ProblemHttpResult;

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        result.ProblemDetails.Detail.Should().Be(someErrorMessage);
    }
}
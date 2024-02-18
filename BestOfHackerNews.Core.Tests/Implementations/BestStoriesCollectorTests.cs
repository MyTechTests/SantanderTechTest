using BestOfHackerNews.Core.Implementations;
using BestOfHackerNews.Core.Interfaces.Config;
using BestOfHackerNews.Core.Interfaces;
using BestOfHackerNews.Core.Records;
using FluentAssertions;
using NSubstitute;

namespace BestOfHackerNews.Core.Tests.Implementations;


[TestClass]
public class BestStoriesCollectorTests
{
    [TestMethod]
    public async Task Start_Should_Initialize_Monitoring()
    {
        // Arrange
        var bestStoriesProvider = Substitute.For<IProvideBestStoriesAsHackerNewsItems>();
        var bestStoriesCollectorConfigProvider = Substitute.For<IBestStoriesCollectorConfigProvider>();
        bestStoriesCollectorConfigProvider.ReadConfig().Returns(new BestStoriesCollectorConfig
        {
            CheckIntervalInSeconds = TimeSpan.FromSeconds(60)
        });

        using var collector = new BestStoriesCollector(bestStoriesProvider, bestStoriesCollectorConfigProvider);

        // Act
        await collector.Start();

        // Assert
        await bestStoriesProvider.Received(1).GetBestStories();
    }

    [TestMethod]
    public async Task Start_Should_Call_GetBestStories_After_Interval()
    {
        // Arrange
        var bestStoriesProvider = Substitute.For<IProvideBestStoriesAsHackerNewsItems>();
        var bestStoriesCollectorConfigProvider = Substitute.For<IBestStoriesCollectorConfigProvider>();
        var interval = TimeSpan.FromMilliseconds(5);
        bestStoriesCollectorConfigProvider.ReadConfig().Returns(new BestStoriesCollectorConfig
        {
            CheckIntervalInSeconds = interval
        });

        using var collector = new BestStoriesCollector(bestStoriesProvider, bestStoriesCollectorConfigProvider);

        var itemsProcessed = 0;
        bestStoriesProvider.When(x => x.GetBestStories())
            .Do(_ => itemsProcessed++);

        // Act
        await collector.Start();

        // Assert
        await Task.Delay(interval.Add(TimeSpan.FromMilliseconds(50)));
        itemsProcessed.Should().BeGreaterOrEqualTo(3);
    }

    [TestMethod]
    public async Task Start_Should_Not_Allow_Multiple_Initializations()
    {
        // Arrange
        var bestStoriesProvider = Substitute.For<IProvideBestStoriesAsHackerNewsItems>();
        var bestStoriesCollectorConfigProvider = Substitute.For<IBestStoriesCollectorConfigProvider>();
        bestStoriesCollectorConfigProvider.ReadConfig().Returns(new BestStoriesCollectorConfig
        {
            CheckIntervalInSeconds = TimeSpan.FromSeconds(60)
        });

        using var collector = new BestStoriesCollector(bestStoriesProvider, bestStoriesCollectorConfigProvider);

        // Act
        await collector.Start();

        // Assert
        await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => collector.Start());
    }

    [TestMethod]
    public async Task Dispose_Should_Dispose_Monitor()
    {
        // Arrange
        var bestStoriesProvider = Substitute.For<IProvideBestStoriesAsHackerNewsItems>();
        var bestStoriesCollectorConfigProvider = Substitute.For<IBestStoriesCollectorConfigProvider>();
        var interval = TimeSpan.FromMilliseconds(5);
        bestStoriesCollectorConfigProvider.ReadConfig().Returns(new BestStoriesCollectorConfig
        {
            CheckIntervalInSeconds = interval
        });

        var collector = new BestStoriesCollector(bestStoriesProvider, bestStoriesCollectorConfigProvider);

        var itemsProcessed = 0;
        bestStoriesProvider.When(x => x.GetBestStories())
            .Do(_ => itemsProcessed++);

        // Act
        await collector.Start();
        collector.Dispose();

        // Assert
        await Task.Delay(interval.Add(TimeSpan.FromMilliseconds(20)));
        itemsProcessed.Should().Be(1);
    }
}
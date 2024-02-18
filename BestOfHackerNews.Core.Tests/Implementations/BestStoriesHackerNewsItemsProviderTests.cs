using System.Collections.Concurrent;
using BestOfHackerNews.Core.Implementations;
using BestOfHackerNews.Core.Interfaces.Config;
using BestOfHackerNews.Core.Interfaces;
using BestOfHackerNews.Core.Records;
using FluentAssertions;
using NSubstitute;
using Flurl.Http.Testing;
using Newtonsoft.Json;

namespace BestOfHackerNews.Core.Tests.Implementations;


[TestClass]
public class BestStoriesHackerNewsItemsProviderTests
{
    [TestMethod]
    public async Task GetBestStories_Should_Retrieve_And_Process_Items()
    {
        // Arrange
        var bestStoriesToHackerNewsItemsProcessor = Substitute.For<IProcessBestStoriesToHackerNewsItems>();
        var storyStore = Substitute.For<IStoreBestStories>();
        var configProvider = Substitute.For<IHackerNewsItemRetrievalConfigProvider>();

        var config = new HackerNewsItemRetrievalConfig
        {
            BestStoriesUri = "https://example.com/best-stories",
            ItemUriFormatString = "https://example.com/item/{0}",
            HttpTimeout = TimeSpan.FromSeconds(30),
            RetryCount = 3,
            RetryDelay = TimeSpan.FromSeconds(5)
        };

        configProvider.ReadConfig().Returns(config);

        var bestStoryIds = new [] { 1 };
        var bestStoriesJson = JsonConvert.SerializeObject(bestStoryIds);
        var hackerNewsItemJson = JsonConvert.SerializeObject(new HackerNewsItem());
        var httpTest = new HttpTest();
        httpTest.ForCallsTo(config.BestStoriesUri).RespondWith(bestStoriesJson);
        httpTest.ForCallsTo(string.Format(config.ItemUriFormatString, "1")).RespondWith(hackerNewsItemJson);

        var provider = new BestStoriesHackerNewsItemsProvider(bestStoriesToHackerNewsItemsProcessor, storyStore, configProvider);

        bestStoriesToHackerNewsItemsProcessor.Process(Arg.Any<HackerNewsItem[]>()).Returns(Array.Empty<Story>());

        // Act
        await provider.GetBestStories();

        // Assert
        bestStoriesToHackerNewsItemsProcessor.Received(1).Process(Arg.Any<HackerNewsItem[]>());
    }

    [TestMethod]
    public async Task GetBestStories_Should_Ignore_Deleted()
    {
        // Arrange
        var bestStoriesToHackerNewsItemsProcessor = Substitute.For<IProcessBestStoriesToHackerNewsItems>();
        var storyStore = Substitute.For<IStoreBestStories>();
        var configProvider = Substitute.For<IHackerNewsItemRetrievalConfigProvider>();

        var config = new HackerNewsItemRetrievalConfig
        {
            BestStoriesUri = "https://example.com/best-stories",
            ItemUriFormatString = "https://example.com/item/{0}",
            HttpTimeout = TimeSpan.FromSeconds(30),
            RetryCount = 3,
            RetryDelay = TimeSpan.FromSeconds(5)
        };

        configProvider.ReadConfig().Returns(config);

        var expected = new HackerNewsItem { id = 1, type = "story", descendants = 32 };

        var bestStoryIds = new [] { 1, 2 };
        var bestStoriesJson = JsonConvert.SerializeObject(bestStoryIds);
        var hackerNewsItemJson = JsonConvert.SerializeObject(new HackerNewsItem { id = 1, type = "story", descendants = 32});

        var httpTest = new HttpTest();
        httpTest.ForCallsTo(config.BestStoriesUri).RespondWith(bestStoriesJson);
        httpTest.ForCallsTo(string.Format(config.ItemUriFormatString, "1")).RespondWith(hackerNewsItemJson);

        var provider = new BestStoriesHackerNewsItemsProvider(bestStoriesToHackerNewsItemsProcessor, storyStore, configProvider);

        var captured = new ConcurrentBag<HackerNewsItem>();
        var _ = bestStoriesToHackerNewsItemsProcessor.Process(Arg.Do<HackerNewsItem[]>(items =>
        {
            foreach (var hackerNewsItem in items)
            {
                captured.Add(hackerNewsItem);
            }
        })).Returns(Array.Empty<Story>());

        // Act
        await provider.GetBestStories();

        // Assert
        captured.Count.Should().Be(1);
        captured.ToArray()[0].Should().BeEquivalentTo(expected);
    }
}
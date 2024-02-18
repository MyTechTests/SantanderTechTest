using BestOfHackerNews.Core.Implementations.Config;
using BestOfHackerNews.Core.Records;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using BestOfHackerNews.Core.Implementations;

namespace BestOfHackerNews.Core.Tests.Implementations.Config;


[TestClass]
public class HackerNewsItemRetrievalConfigProviderTests
{
    [TestMethod]
    public void ReadConfig_Should_Return_Correct_Configuration()
    {
        // Arrange
        var expectedConfig = new HackerNewsItemRetrievalConfig
        {
            BestStoriesUri = "https://example.com/best-stories",
            HttpTimeout = TimeSpan.FromSeconds(30),
            ItemUriFormatString = "https://example.com/item/{0}",
            MaxItemIdUri = "https://example.com/max-item-id",
            RetryCount = 3,
            RetryDelay = TimeSpan.FromSeconds(5)
        };

        var inMemorySettings = new Dictionary<string, string>
        {
            { Constants.Configuration.SectionName + ":" + Constants.Configuration.BestStoriesUri, expectedConfig.BestStoriesUri },
            { Constants.Configuration.SectionName + ":" + Constants.Configuration.HttpTimeoutMs, ((int)expectedConfig.HttpTimeout.TotalMilliseconds).ToString() },
            { Constants.Configuration.SectionName + ":" + Constants.Configuration.ItemUriFormatString, expectedConfig.ItemUriFormatString },
            { Constants.Configuration.SectionName + ":" + Constants.Configuration.MaxItemIdUri, expectedConfig.MaxItemIdUri },
            { Constants.Configuration.SectionName + ":" + Constants.Configuration.NewsApiRetryCount, expectedConfig.RetryCount.ToString() },
            { Constants.Configuration.SectionName + ":" + Constants.Configuration.NewsApiRetryDelayMs, ((int)expectedConfig.RetryDelay.TotalMilliseconds).ToString() }
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        var provider = new HackerNewsItemRetrievalConfigProvider(configuration);

        // Act
        var result = provider.ReadConfig();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedConfig);
    }
}
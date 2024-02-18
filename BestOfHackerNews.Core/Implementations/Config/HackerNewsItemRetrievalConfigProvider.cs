using BestOfHackerNews.Core.Interfaces.Config;
using BestOfHackerNews.Core.Records;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace BestOfHackerNews.Core.Implementations.Config;

/// <summary>
/// Gets an object that contains settings required for the Best Stories Collector
/// </summary>
internal class HackerNewsItemRetrievalConfigProvider(IConfiguration config) : IHackerNewsItemRetrievalConfigProvider
{
    private const string MISSING = "MISSING";

    public HackerNewsItemRetrievalConfig ReadConfig()
    {
        Log.Information("Getting configuration settings for Best stories collector...");
        var configurationSection = config.GetSection(Constants.Configuration.SectionName);
        var bestStoriesCollectorConfig = new HackerNewsItemRetrievalConfig
        {
            BestStoriesUri = configurationSection.GetValue<string>(Constants.Configuration.BestStoriesUri) ?? MISSING,
            HttpTimeout = TimeSpan.FromMilliseconds(configurationSection.GetValue<int>(Constants.Configuration.HttpTimeoutMs)),
            ItemUriFormatString = configurationSection.GetValue<string>(Constants.Configuration.ItemUriFormatString) ?? MISSING,
            MaxItemIdUri = configurationSection.GetValue<string>(Constants.Configuration.MaxItemIdUri) ?? MISSING,
            RetryCount = configurationSection.GetValue<int>(Constants.Configuration.NewsApiRetryCount),
            RetryDelay = TimeSpan.FromMilliseconds(configurationSection.GetValue<int>(Constants.Configuration.NewsApiRetryDelayMs))
        };

        Log.Information("Best stories collector initialising with the following settings: {@config}", bestStoriesCollectorConfig);

        return bestStoriesCollectorConfig;
    }
}
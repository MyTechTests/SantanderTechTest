﻿using BestOfHackerNews.Core.Interfaces.Config;
using BestOfHackerNews.Core.Records;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace BestOfHackerNews.Core.Implementations.Config;

/// <summary>
/// Gets an object that contains settings required for the Best Stories Collector
/// </summary>
internal class BestStoriesCollectorConfigProvider : IBestStoriesCollectorConfigProvider
{
    private const string MISSING = "MISSING";
    private readonly IConfiguration _config;

    public BestStoriesCollectorConfigProvider(IConfiguration config)
    {
        _config = config;
    }

    public BestStoriesCollectorConfig ReadConfig()
    {
        Log.Information("Getting configuration settings for Best stories collector...");
        var configurationSection = _config.GetSection(Constants.Configuration.SectionName);
        var bestStoriesCollectorConfig = new BestStoriesCollectorConfig
        {
            CheckIntervalInSeconds = TimeSpan.FromSeconds(configurationSection.GetValue<int>(Constants.Configuration.NewsApiCheckIntervalInSeconds))
        };

        Log.Information("Best stories collector initialising with the following settings: {@config}", bestStoriesCollectorConfig);

        return bestStoriesCollectorConfig;
    }
}
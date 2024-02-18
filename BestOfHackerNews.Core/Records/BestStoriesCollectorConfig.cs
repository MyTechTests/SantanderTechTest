namespace BestOfHackerNews.Core.Records;

/// <summary>
/// Configuration for the BestStoriesCollector
/// </summary>
internal record BestStoriesCollectorConfig
{
    public TimeSpan CheckIntervalInSeconds { get; set; }
}
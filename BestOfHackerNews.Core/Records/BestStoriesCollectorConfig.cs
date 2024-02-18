namespace BestOfHackerNews.Core.Records;

public record BestStoriesCollectorConfig
{
    public TimeSpan CheckIntervalInSeconds { get; set; }
}
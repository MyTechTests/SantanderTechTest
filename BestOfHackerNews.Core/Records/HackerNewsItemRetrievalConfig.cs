namespace BestOfHackerNews.Core.Records;

public record HackerNewsItemRetrievalConfig
{
    private static readonly string MISSING = "MISSING";

    public int RetryCount { get; set; }
    public TimeSpan RetryDelay { get; set; }
    public string BestStoriesUri { get; set; } = MISSING;
    public string ItemUriFormatString { get; set; } = MISSING;
    public string MaxItemIdUri { get; set; } = MISSING;
    public TimeSpan HttpTimeout { get; set; }
}
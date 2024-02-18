namespace BestOfHackerNews.Core.Implementations;

/// <summary>
/// All constants used in the service
/// </summary>
/// <remarks>
/// Values are public, so cannot be const as that can cause issues with differing assembly versions.  Using static readonly as it is essentially the same.
/// </remarks>
public static class Constants
{
    /// <summary>
    /// All configuration constants
    /// </summary>
    public static class Configuration
    {
        public static readonly string BestStoriesUri = "BestStoriesUri";
        public static readonly string HttpTimeoutMs = "HttpTimeoutMs";
        public static readonly string ItemUriFormatString = "ItemUriFormatString";
        public static readonly string MaxItemIdUri = "MaxItemIdUri";
        public static readonly string NewsApiCheckIntervalInSeconds = "NewsApiCheckIntervalInSeconds";
        public static readonly string NewsApiRetryCount = "NewsApiRetryCount";
        public static readonly string NewsApiRetryDelayMs = "NewsApiRetryDelayMs";
        public static readonly string SectionName = "Settings";
    }

    /// <summary>
    /// All constants used for serving data to clients
    /// </summary>
    public static class Server
    {
        public static readonly string ApiKey = "ApiKey";
    }
}
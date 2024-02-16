namespace BestOfHackerNews.Core.Implementations;

/// <summary>
/// All constants used in the service
/// </summary>
public static class Constants
{
    /// <summary>
    /// All configuration constants
    /// </summary>
    public static class Configuration
    {
        public static readonly string BestStoriesUri = "BestStoriesUri";
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
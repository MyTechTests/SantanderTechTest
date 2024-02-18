namespace BestOfHackerNews.Core.Interfaces;

/// <summary>
/// Monitors the hacker news api and stores updates
/// </summary>
internal interface ICollectBestStories
{
    Task Start();
}
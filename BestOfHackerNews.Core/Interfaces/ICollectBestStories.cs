namespace BestOfHackerNews.Core.Interfaces;

/// <summary>
/// Monitors the hacker news api and stores updates
/// </summary>
public interface ICollectBestStories
{
    Task BeginCollection();
}
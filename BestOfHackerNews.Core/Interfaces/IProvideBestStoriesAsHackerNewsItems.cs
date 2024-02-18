namespace BestOfHackerNews.Core.Interfaces;

/// <summary>
/// Provides the best stories from the hacker news site
/// </summary>
internal interface IProvideBestStoriesAsHackerNewsItems
{
    Task GetBestStories();
}
namespace BestOfHackerNews.Core.Interfaces;

public interface IProvideBestStoriesAsHackerNewsItems
{
    Task GetBestStories();
}
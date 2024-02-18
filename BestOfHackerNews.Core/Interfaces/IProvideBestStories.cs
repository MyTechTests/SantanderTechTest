using BestOfHackerNews.Core.Records;

namespace BestOfHackerNews.Core.Interfaces;

/// <summary>
/// Provides stories sourced from Hacker News
/// </summary>
public interface IProvideBestStories
{
    /// <summary>
    /// Gets the top n stories by popularity
    /// </summary>
    /// <param name="storyCount">The number of stories to return in order of most popular first</param>
    /// <returns>An array of stories</returns>
    Story[] GetBestStories(int storyCount);
}
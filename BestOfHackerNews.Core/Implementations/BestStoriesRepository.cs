using BestOfHackerNews.Core.Interfaces;
using BestOfHackerNews.Core.Records;

namespace BestOfHackerNews.Core.Implementations;

/// <summary>
/// Repository for stories.  Can be replaced by a database store if required.
/// </summary>
internal class BestStoriesRepository : IProvideBestStories, IStoreBestStories
{
    private static Story[] _bestStories = Array.Empty<Story>();

    public Story[] GetBestStories(int storyCount)
    {
        return _bestStories.Take(storyCount).ToArray();
    }

    public void Update(Story[] stories)
    {
        _bestStories = stories.OrderByDescending(o => o.score).ToArray();
    }
}
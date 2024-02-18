using BestOfHackerNews.Core.Interfaces;
using BestOfHackerNews.Core.Records;

namespace BestOfHackerNews.Core.Implementations;

/// <summary>
/// Repository for stories.  Can be replaced by a database store if required.
/// </summary>
/// <remarks>
/// This class effectively decouples the consumption rate from the update rate
/// as the two methods can be called irrespective of each other.
/// CLR guarantee means you will not get threading issue when the _bestStories
/// is reassigned.  No locking is required
/// </remarks>
internal class BestStoriesRepository : IProvideBestStories, IStoreBestStories
{
    private static Story[] _bestStories = Array.Empty<Story>();

    public Story[] GetBestStories(int storyCount)
    {
        return _bestStories.Take(storyCount).ToArray();
    }

    public void ReplaceAll(Story[] stories)
    {
        _bestStories = stories;
    }
}
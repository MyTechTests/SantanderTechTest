using BestOfHackerNews.Core.Interfaces;
using BestOfHackerNews.Core.Records;

namespace BestOfHackerNews.Core.Implementations;

internal class BestStoriesToHackerNewsItemsProcessor : IProcessBestStoriesToHackerNewsItems
{
    public Story[] Process(HackerNewsItem[] hackerNewsItems)
    {
        var stories = hackerNewsItems
            .Where(o => !ReferenceEquals(o, null))
            .Select(ConvertToStory)
            .OrderByDescending(o => o.score)
            .ToArray();

        return stories;
    }

    private static Story ConvertToStory(HackerNewsItem hackerNewsItem)
    {
        var story = new Story
        {
            title = hackerNewsItem.title,
            uri = hackerNewsItem.url,
            postedBy = hackerNewsItem.by,
            time = DateTimeOffset.FromUnixTimeSeconds(hackerNewsItem.time).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss+00:00"),
            score = hackerNewsItem.score,
            commentCount = hackerNewsItem.descendants
        };

        return story;
    }
}
using BestOfHackerNews.Core.Records;

namespace BestOfHackerNews.Core.Interfaces;

/// <summary>
/// Applies filtering, sorting and conversion of hackerNewsItems
/// </summary>
internal interface IProcessBestStoriesToHackerNewsItems
{
    Story[] Process(HackerNewsItem[] hackerNewsItems);
}
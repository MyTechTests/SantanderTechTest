using BestOfHackerNews.Core.Records;

namespace BestOfHackerNews.Core.Interfaces;

public interface IProcessBestStoriesToHackerNewsItems
{
    Story[] Process(HackerNewsItem[] results);
}
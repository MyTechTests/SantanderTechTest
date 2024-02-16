using BestOfHackerNews.Core.Records;

namespace BestOfHackerNews.Core.Interfaces;

internal interface IStoreBestStories
{
    void Update(Story[] stories);
}
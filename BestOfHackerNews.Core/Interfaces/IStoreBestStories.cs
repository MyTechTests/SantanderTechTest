using BestOfHackerNews.Core.Records;

namespace BestOfHackerNews.Core.Interfaces;

public interface IStoreBestStories
{
    void Update(Story[] stories);
}
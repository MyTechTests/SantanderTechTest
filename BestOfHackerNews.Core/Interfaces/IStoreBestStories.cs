using BestOfHackerNews.Core.Records;

namespace BestOfHackerNews.Core.Interfaces;

/// <summary>
/// Updates the story store with 
/// </summary>
internal interface IStoreBestStories
{
    void ReplaceAll(Story[] stories);
}
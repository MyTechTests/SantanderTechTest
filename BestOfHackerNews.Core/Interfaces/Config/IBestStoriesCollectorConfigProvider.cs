using BestOfHackerNews.Core.Records;

namespace BestOfHackerNews.Core.Interfaces.Config;

internal interface IBestStoriesCollectorConfigProvider
{
    BestStoriesCollectorConfig? ReadConfig();
}
using BestOfHackerNews.Core.Records;

namespace BestOfHackerNews.Core.Interfaces.Config;

public interface IBestStoriesCollectorConfigProvider
{
    BestStoriesCollectorConfig ReadConfig();
}
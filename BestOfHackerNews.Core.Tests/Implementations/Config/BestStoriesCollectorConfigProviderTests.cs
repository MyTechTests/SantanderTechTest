using BestOfHackerNews.Core.Implementations;
using BestOfHackerNews.Core.Implementations.Config;
using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace BestOfHackerNews.Core.Tests.Implementations.Config;


[TestClass]
public class BestStoriesCollectorConfigProviderTests
{
    [TestMethod]
    public void ReadConfig_Should_Return_Correct_Configuration()
    {
        // Arrange
        var expectedInterval = TimeSpan.FromSeconds(60);
        var inMemorySettings = new Dictionary<string, string> {
            { Constants.Configuration.SectionName + ":" + Constants.Configuration.NewsApiCheckIntervalInSeconds, "60"}
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        var provider = new BestStoriesCollectorConfigProvider(configuration);

        // Act
        var result = provider.ReadConfig();

        // Assert
        result.Should().NotBeNull();
        result!.CheckIntervalInSeconds.Should().Be(expectedInterval);
    }
}
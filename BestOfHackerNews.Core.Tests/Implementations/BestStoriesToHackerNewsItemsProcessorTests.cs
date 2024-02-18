using BestOfHackerNews.Core.Implementations;
using BestOfHackerNews.Core.Records;
using FluentAssertions;

namespace BestOfHackerNews.Core.Tests.Implementations;


[TestClass]
public class BestStoriesToHackerNewsItemsProcessorTests
{
    [TestMethod]
    public void Process_Should_Convert_HackerNewsItems_To_Stories()
    {
        // Arrange
        var processor = new BestStoriesToHackerNewsItemsProcessor();
        var expected = new[]
        {
            new Story
            {
                title = "A uBlock Origin update was rejected from the Chrome Web Store",
                uri = "https://github.com/uBlockOrigin/uBlock-issues/issues/745",
                postedBy = "ismaildonmez",
                time = "2019-10-12T13:43:01+00:00",
                score = 1716,
                commentCount = 572
            },
            new Story
            {
                title = "Some story",
                uri = "http://www.google.co.uk",
                postedBy = "ismaildonmez",
                time = "2018-10-12T13:43:01+00:00",
                score = 716,
                commentCount = 111
            }
        };

        var hackerNewsItems = new[]
        {
            new HackerNewsItem
            {
                id = 1,
                by = "ismaildonmez",
                descendants = 111,
                score = 716,
                time = DateTimeOffset.Parse("2018-10-12T13:43:01+00:00").ToUnixTimeSeconds(),
                title = "Some story",
                url = "http://www.google.co.uk"
            },
            new HackerNewsItem
            {
                id = 2,
                by = "ismaildonmez",
                descendants = 572,
                score = 1716,
                time = DateTimeOffset.Parse("2019-10-12T13:43:01+00:00").ToUnixTimeSeconds(),
                title = "A uBlock Origin update was rejected from the Chrome Web Store",
                url = "https://github.com/uBlockOrigin/uBlock-issues/issues/745"
            }
        };

        // Act
        var result = processor.Process(hackerNewsItems);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}
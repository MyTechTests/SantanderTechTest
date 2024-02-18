using BestOfHackerNews.Core.Implementations;
using BestOfHackerNews.Core.Records;

namespace BestOfHackerNews.Core.Tests.Implementations;


[TestClass]
public class BestStoriesRepositoryTests
{
    [TestMethod]
    public void GetBestStories_Should_Return_Empty_Array_When_No_Stories_Available()
    {
        // Arrange
        var repository = new BestStoriesRepository();

        // Act
        var stories = repository.GetBestStories(5);

        // Assert
        Assert.IsNotNull(stories);
        Assert.AreEqual(0, stories.Length);
    }

    [TestMethod]
    public void GetBestStories_Should_Return_Requested_Number_Of_Stories()
    {
        // Arrange
        var repository = new BestStoriesRepository();
        var stories = new[]
        {
            new Story { title = "Story 1", score = 10 },
            new Story { title = "Story 2", score = 20 },
            new Story { title = "Story 3", score = 15 }
        };
        repository.ReplaceAll(stories);

        // Act
        var requestedStories = repository.GetBestStories(2);

        // Assert
        Assert.IsNotNull(requestedStories);
        Assert.AreEqual(2, requestedStories.Length);
    }
}
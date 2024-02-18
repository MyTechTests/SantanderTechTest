// ReSharper disable InconsistentNaming
//Naming is cased to match expected output when deserialised
namespace BestOfHackerNews.Core.Records;

/// <summary>
/// Represents a story
/// </summary>
public record Story
{
    /// <summary>
    /// The title of the story.
    /// </summary>
    public string? title { get; set; }

    /// <summary>
    /// The uri of the story.
    /// </summary>
    public string? uri { get; set; }

    /// <summary>
    /// The username on the hacker news site of the author of the story.
    /// </summary>
    public string? postedBy { get; set; }

    /// <summary>
    /// When the story was created
    /// </summary>
    public string? time { get; set; }

    /// <summary>
    /// The score of this story on the hacker news site
    /// </summary>
    public int score { get; set; }

    /// <summary>
    /// The total comment count for story.
    /// </summary>
    public int commentCount { get; set; }
}
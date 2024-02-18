namespace BestOfHackerNews.Core.Records;

/// <summary>
/// Represents an item from Hacker News.
/// </summary>
public record HackerNewsItem
{
    /// <summary>
    /// Gets or sets the item's unique id.
    /// </summary>
    public int id { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the item is deleted.
    /// </summary>
    public bool deleted { get; set; }

    /// <summary>
    /// Gets or sets the type of item. One of "job", "story", "comment", "poll", or "pollopt".
    /// </summary>
    public string? type { get; set; }

    /// <summary>
    /// Gets or sets the username of the item's author.
    /// </summary>
    public string? by { get; set; }

    /// <summary>
    /// Gets or sets the creation date of the item, in Unix Time.
    /// </summary>
    public long time { get; set; }

    /// <summary>
    /// Gets or sets the comment, story, or poll text. HTML.
    /// </summary>
    public string? text { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the item is dead.
    /// </summary>
    public bool dead { get; set; }

    /// <summary>
    /// Gets or sets the comment's parent: either another comment or the relevant story.
    /// </summary>
    public int parent { get; set; }

    /// <summary>
    /// Gets or sets the pollopt's associated poll.
    /// </summary>
    public int poll { get; set; }

    /// <summary>
    /// Gets or sets the ids of the item's comments, in ranked display order.
    /// </summary>
    public int[]? kids { get; set; }

    /// <summary>
    /// Gets or sets the URL of the story.
    /// </summary>
    public string? url { get; set; }

    /// <summary>
    /// Gets or sets the story's score, or the votes for a pollopt.
    /// </summary>
    public int score { get; set; }

    /// <summary>
    /// Gets or sets the title of the story, poll, or job. HTML.
    /// </summary>
    public string? title { get; set; }

    /// <summary>
    /// Gets or sets a list of related pollopts, in display order.
    /// </summary>
    public int[]? parts { get; set; }

    /// <summary>
    /// Gets or sets the total comment count for stories or polls.
    /// </summary>
    public int descendants { get; set; }
}
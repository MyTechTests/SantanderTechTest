/// <summary>
/// Represents an item from Hacker News.
/// </summary>
public record HackerNewsItem
{
    /// <summary>
    /// Gets or sets the item's unique id.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether the item is deleted.
    /// </summary>
    public bool Deleted { get; init; }

    /// <summary>
    /// Gets or sets the type of item. One of "job", "story", "comment", "poll", or "pollopt".
    /// </summary>
    public string Type { get; init; }

    /// <summary>
    /// Gets or sets the username of the item's author.
    /// </summary>
    public string By { get; init; }

    /// <summary>
    /// Gets or sets the creation date of the item, in Unix Time.
    /// </summary>
    public DateTimeOffset Time { get; init; }

    /// <summary>
    /// Gets or sets the comment, story, or poll text. HTML.
    /// </summary>
    public string Text { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether the item is dead.
    /// </summary>
    public bool Dead { get; init; }

    /// <summary>
    /// Gets or sets the comment's parent: either another comment or the relevant story.
    /// </summary>
    public int Parent { get; init; }

    /// <summary>
    /// Gets or sets the pollopt's associated poll.
    /// </summary>
    public int Poll { get; init; }

    /// <summary>
    /// Gets or sets the ids of the item's comments, in ranked display order.
    /// </summary>
    public int[] Kids { get; init; }

    /// <summary>
    /// Gets or sets the URL of the story.
    /// </summary>
    public string Url { get; init; }

    /// <summary>
    /// Gets or sets the story's score, or the votes for a pollopt.
    /// </summary>
    public int Score { get; init; }

    /// <summary>
    /// Gets or sets the title of the story, poll, or job. HTML.
    /// </summary>
    public string Title { get; init; }

    /// <summary>
    /// Gets or sets a list of related pollopts, in display order.
    /// </summary>
    public int[] Parts { get; init; }

    /// <summary>
    /// Gets or sets the total comment count for stories or polls.
    /// </summary>
    public int Descendants { get; init; }
}

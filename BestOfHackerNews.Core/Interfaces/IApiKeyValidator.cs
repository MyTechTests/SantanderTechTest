namespace BestOfHackerNews.Core.Interfaces;

/// <summary>
/// Validates an api key and returns success, an appropriate http code and a message
/// </summary>
public interface IApiKeyValidator
{
    (bool success, int httpStatusCode, string httpStatusMessage) Validate(string apiKey);
}
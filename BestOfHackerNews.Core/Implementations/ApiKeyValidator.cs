using BestOfHackerNews.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace BestOfHackerNews.Core.Implementations;

/// <summary>
/// Validates api keys provided in the header of http requests
/// </summary>
public class ApiKeyValidator : IApiKeyValidator
{
    private readonly Dictionary<string, string?> _apiKeys;

    /// <summary>
    /// Initialises the validator with known keys
    /// </summary>
    /// <param name="configuration"></param>
    public ApiKeyValidator(IConfiguration configuration)
    {
        var apiKeysSection = configuration.GetSection("ApiKeys");

        _apiKeys = apiKeysSection
            .GetChildren()
            .ToDictionary(x => x.Key, x => x.Value);
    }

    /// <summary>
    /// Validates an API Key and returns a detailed result
    /// </summary>
    /// <param name="apiKey">The key to be tested</param>
    /// <returns>success indicator, the http status code and the status message from the http response</returns>
    public (bool success, int httpStatusCode, string httpStatusMessage) Validate(string? apiKey)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return (false, StatusCodes.Status401Unauthorized, "An API key is required, but was not supplied");
        }

        if (!_apiKeys.ContainsKey(apiKey))
        {
            return (false, StatusCodes.Status401Unauthorized, "The supplied API key is invalid");
        }

        return (true, StatusCodes.Status200OK, "Successfully validated the supplied api key");
    }
}
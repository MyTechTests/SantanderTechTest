using BestOfHackerNews.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace BestOfHackerNews.Core.Implementations;

public class ApiKeyValidator : IApiKeyValidator
{
    private readonly Dictionary<string, string?> _apiKeys;

    public ApiKeyValidator(IConfiguration configuration)
    {
        var apiKeysSection = configuration.GetSection("ApiKeys");

        _apiKeys = apiKeysSection
            .GetChildren()
            .ToDictionary(x => x.Key, x => x.Value);
    }

    public (bool success, int httpStatusCode, string httpStatusMessage) Validate(string apiKey)
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
using BestOfHackerNews.Core.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace BestOfHackerNews.Core.Extensions;

/// <summary>
/// Additional support for builder configuration
/// </summary>
public static class BohnApplicationBuilderExtensions //required prefix as this clashed with a microsoft class of the same name
{
    /// <summary>
    /// Adds the api key checking middleware
    /// </summary>
    /// <param name="app">The application builder to configure</param>
    /// <returns>The application builder</returns>
    public static IApplicationBuilder RequireApiKey(this IApplicationBuilder app)
    {
        return app.Use(CheckApiKey);
    }

    internal static Task CheckApiKey(HttpContext context, Func<Task> next)
    {
        var apiKey = context.Request.Headers["X-API-KEY"];

        var apiKeyValidator = context.RequestServices.GetRequiredService<IApiKeyValidator>();
        var (success, httpStatusCode, httpStatusMessage) = apiKeyValidator.Validate(apiKey!);

        if (!success)
        {
            Log.Warning("Api key '{apiKey}' not recognised {@time}", apiKey, DateTime.UtcNow);
            context.Response.StatusCode = httpStatusCode;
            context.Response.WriteAsync(httpStatusMessage);
            return Task.CompletedTask;
        }

        Log.Information("Api key '{apiKey}' verified as ok {@time}", apiKey, DateTime.UtcNow);
        return next.Invoke();
    }
}
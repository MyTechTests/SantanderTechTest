using BestOfHackerNews.Core.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace BestOfHackerNews.Core.Extensions;

/// <summary>
/// This is where endpoints are mapped
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Initialises monitoring of the hacker news api
    /// </summary>
    /// <param name="app">The web application containing the ICollectBestStories instance</param>
    /// <returns>The web application</returns>
    public static async Task<WebApplication> BeginListeningToBohn(this WebApplication app)
    {
        var bestStoriesCollector = app.Services.GetRequiredService<ICollectBestStories>();

        await bestStoriesCollector.Start();

        return app;
    }

    /// <summary>
    /// Adds the endpoint for retrieving the best n stories from hacker news
    /// </summary>
    /// <param name="app">The WebApplication we will add the mapping to for this route</param>
    /// <returns>The best n stories from hacker news as a Json array</returns>
    public static WebApplication AddBohnGetEndpoint(this WebApplication app)
    {
        app.MapGet(
                "/bestnstories/{count}", 
                BohnGetHandler)
            .WithName("Get Best Stories")
            .WithOpenApi();

        return app;
    }

    internal static IResult BohnGetHandler(int count, HttpResponse response, HttpContext context)
    {
        Log.Information("{endpointName} called on route {route} (method '{caller}') called at {@time}", "Get Best Stories", "/bestnstories/{count}", null, DateTime.UtcNow);

        if (count <= 0)
        {
            return Results.Problem(statusCode: StatusCodes.Status406NotAcceptable, detail: "The number of stories requested must be at least one.");
        }

        try
        {
            var bestStoriesProvider = context.RequestServices.GetRequiredService<IProvideBestStories>();

            var result = bestStoriesProvider.GetBestStories(count);

            return result.Length == 0 ?
                Results.NoContent() :
                Results.Ok(result);
        }
        catch (Exception ex)
        {
            Log.Error("{endpointName} called on route {route} (method '{caller}') failed at {@time} with {@exception}", "Get Best Stories", "/bestnstories/{count}", null, DateTime.UtcNow, ex);
            var problem = Results.Problem(statusCode: StatusCodes.Status500InternalServerError, detail: ex.Message);
            return problem;
        }
        finally
        {
            Log.Information("{endpointName} called on route {route} (method '{caller}') completed at {@time}", "Get Best Stories", "/bestnstories/{count}", null, DateTime.UtcNow);
        }
    }
}
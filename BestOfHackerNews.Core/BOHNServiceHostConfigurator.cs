using BestOfHackerNews.Core.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BestOfHackerNews.Core;

/// <summary>
/// Configures the services required by this api for a host
/// </summary>
public static class BohnServiceHostConfigurator
{
    /// <summary>
    /// Configures an existing WebApplicationBuilder to add support for this api
    /// </summary>
    /// <remarks>Environment variable prefix used is "BOHN_"</remarks>
    /// <param name="builder">The builder to configure</param>
    public static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Configuration.AddEnvironmentVariables(prefix: "BOHN_");

        builder.Services
            .AddEndpointsApiExplorer()
            .AddCoreComponentsForBohn()
            .AddCommonRateLimiter()
            .AddSwaggerGenWithApiKeyRequirementForUi();
    }

    /// <summary>
    /// Configures an existing WebApplication to add support for this api
    /// </summary>
    /// <param name="app">The app to configure</param>
    public static void ConfigureApplication(WebApplication app)
    {
        app.UseSwagger()
            .UseSwaggerUI()
            .UseHttpsRedirection()
            .UseStatusCodePages()
            .UseRateLimiter()
            .RequireApiKey();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.AddBohnGetEndpoint();
    }
}
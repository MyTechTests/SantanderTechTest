using System.Threading.RateLimiting;
using BestOfHackerNews.Core.Implementations;
using BestOfHackerNews.Core.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
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
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Configuration.AddEnvironmentVariables(prefix: "BOHN_");
        builder.Services.AddSingleton<IApiKeyValidator, ApiKeyValidator>();
        builder.Services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
            {
                return RateLimitPartition.GetFixedWindowLimiter(partitionKey: context.Request.Headers.Host.ToString(), _ =>
                    new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 5,
                        AutoReplenishment = true,
                        Window = TimeSpan.FromSeconds(10)
                    });
            });

            options.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.HttpContext.Response.WriteAsync("Too many requests. Please try later again... ", cancellationToken: token);
            };
        });
    }

    /// <summary>
    /// Configures an existing WebApplication to add support for this api
    /// </summary>
    /// <param name="app">The app to configure</param>
    public static void ConfigureApplication(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        
        app.UseRateLimiter();

        app.Use((context, next) =>
        {
            var apiKey = context.Request.Headers["X-API-KEY"];

            var apiKeyValidator = context.RequestServices.GetRequiredService<IApiKeyValidator>();
            var (success, httpStatusCode, httpStatusMessage) = apiKeyValidator.Validate(apiKey!);

            if (!success)
            {
                context.Response.StatusCode = httpStatusCode;
                context.Response.WriteAsync(httpStatusMessage);
                return Task.CompletedTask;
            }

            return next.Invoke();
        });

        app.MapGet("/beststories", async context =>
            {
                try
                {
                    //var bestStoriesProvider = context.RequestServices.GetRequiredService<IGetBestStoriesFromHackerNews>();
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    await context.Response.WriteAsync("Success!");
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync(ex.ToString());
                }
            })
            .WithName("GetBestStories")
            .WithOpenApi();
    }
}
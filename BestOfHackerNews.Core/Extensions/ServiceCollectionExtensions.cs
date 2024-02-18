using System.Threading.RateLimiting;
using BestOfHackerNews.Core.Implementations;
using BestOfHackerNews.Core.Implementations.Config;
using BestOfHackerNews.Core.Interfaces;
using BestOfHackerNews.Core.Interfaces.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Serilog;

namespace BestOfHackerNews.Core.Extensions;

/// <summary>
/// Additional support for service collection configuration
/// </summary>
/// <remarks>AddCommonRateLimiter is tested under ProgramTests AddCommonRateLimiter_Should_Return_429_TooManyRequests_When_Exceeding_Rate_Limit</remarks>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers all the business logic components required by the service
    /// </summary>
    /// <param name="services">The service collection to add to</param>
    /// <returns>The updated service collection </returns>
    public static IServiceCollection AddCoreComponentsForBohn(this IServiceCollection services)
    {
        var bestStoriesRepository = new BestStoriesRepository();

        return services
            .AddSingleton<IApiKeyValidator, ApiKeyValidator>()
            .AddSingleton<IBestStoriesCollectorConfigProvider, BestStoriesCollectorConfigProvider>()
            .AddSingleton<ICollectBestStories, BestStoriesCollector>()
            .AddSingleton<IHackerNewsItemRetrievalConfigProvider, HackerNewsItemRetrievalConfigProvider>()
            .AddSingleton<IProcessBestStoriesToHackerNewsItems, BestStoriesToHackerNewsItemsProcessor>()
            .AddSingleton<IProvideBestStories>(bestStoriesRepository)
            .AddSingleton<IProvideBestStoriesAsHackerNewsItems, BestStoriesHackerNewsItemsProvider>()
            .AddSingleton<IStoreBestStories>(bestStoriesRepository);
    }

    /// <summary>
    /// Adds global rate limiting to defend against DoS attacks.
    /// </summary>
    /// <param name="services">The service collection to add this feature to</param>
    /// <returns>The updated service collection</returns>
    /// <remarks>Sourced from here: https://blog.maartenballiauw.be/post/2022/09/26/aspnet-core-rate-limiting-middleware.html</remarks>
    public static IServiceCollection AddCommonRateLimiter(this IServiceCollection services)
    {
        return services.AddRateLimiter(options =>
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
                Log.Information("Client '{host}' made too many requests and was blocked at {@time}", context.HttpContext.Request.Headers.Host.ToString(), DateTime.UtcNow);
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.HttpContext.Response.WriteAsync("Too many requests. Please try later again... ", cancellationToken: token);
            };
        });
    }

    /// <summary>
    /// Calls SwaggerGen but with configuration required to support ApiKey on the ui
    /// </summary>
    /// <param name="services"></param>
    /// <returns>The updated service collection </returns>
    public static IServiceCollection AddSwaggerGenWithApiKeyRequirementForUi(this IServiceCollection services)
    {
        return services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Best of Hacker News Service", Version = "1" });
            c.AddSecurityDefinition(Constants.Server.ApiKey, new OpenApiSecurityScheme
            {
                Name = "x-api-key",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Description = "Authorization by x-api-key inside request's header",
                Scheme = "ApiKeyScheme"
            });

            var key = new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = Constants.Server.ApiKey
                },
                In = ParameterLocation.Header
            };

            var requirement = new OpenApiSecurityRequirement { { key, new List<string>() } };

            c.AddSecurityRequirement(requirement);
        });
    }
}
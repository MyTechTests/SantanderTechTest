using BestOfHackerNews.Core.Interfaces.Config;
using BestOfHackerNews.Core.Interfaces;
using BestOfHackerNews.Core.Records;
using Flurl.Http;
using Polly.Retry;
using Polly;
using Serilog;
using System.Collections.Concurrent;

namespace BestOfHackerNews.Core.Implementations;

/// <summary>
/// Sources the story information from teh hacker news site
/// </summary>
internal class BestStoriesHackerNewsItemsProvider : IProvideBestStoriesAsHackerNewsItems
{
    private readonly AsyncRetryPolicy _retryPolicy;
    private readonly IProcessBestStoriesToHackerNewsItems _bestStoriesToHackerNewsItemsProcessor;
    private readonly IStoreBestStories _storyStore;
    private readonly HackerNewsItemRetrievalConfig _hackerNewsItemsProcessorConfig;

    public BestStoriesHackerNewsItemsProvider(
        IProcessBestStoriesToHackerNewsItems bestStoriesToHackerNewsItemsProcessor,
        IStoreBestStories storyStore,
        IHackerNewsItemRetrievalConfigProvider configProvider)
    {
        _bestStoriesToHackerNewsItemsProcessor = bestStoriesToHackerNewsItemsProcessor;
        _storyStore = storyStore;

        _hackerNewsItemsProcessorConfig = configProvider.ReadConfig();
        _retryPolicy = BuildHttpRetryPolicy();
    }

    private AsyncRetryPolicy BuildHttpRetryPolicy()
    {
        var buildHttpRetryPolicy = Policy
            .Handle<FlurlHttpException>()
            .WaitAndRetryAsync(_hackerNewsItemsProcessorConfig.RetryCount, count =>
            {
                Log.Warning("Retry attempt {count}", count);
                return _hackerNewsItemsProcessorConfig.RetryDelay;
            });

        return buildHttpRetryPolicy;
    }

    private async Task<T?> GetFromUrl<T>(string uri)
    {
        var result = await _retryPolicy.ExecuteAndCaptureAsync(() =>
            uri.WithTimeout(_hackerNewsItemsProcessorConfig.HttpTimeout).GetJsonAsync<T>());

        if (result.Outcome != OutcomeType.Successful)
        {
            Log.Warning("Call to {uri} failed: {@exception}", uri, result.FinalException);
        }

        return result.Result;
    }

    public async Task GetBestStories()
    {
        Log.Information("Checking for updates...");

        var storyIds = await GetDistinctBestStoryIds();

        await GetHackerNewsItems(storyIds);
    }

    private async Task<int[]> GetDistinctBestStoryIds()
    {
        var stories = await GetFromUrl<int[]>(_hackerNewsItemsProcessorConfig.BestStoriesUri);

        return stories?.Distinct().ToArray() ?? Array.Empty<int>();
    }

    private async Task GetHackerNewsItems(int[] storyIds)
    {
        var hackerNewsItemBag = new ConcurrentBag<HackerNewsItem>();
        var tasks = storyIds
            .Select(async o =>
            {
                var uri = string.Format(_hackerNewsItemsProcessorConfig.ItemUriFormatString, o);
                var result = await GetFromUrl<HackerNewsItem>(uri);

                if (result == null || result.deleted) return;

                hackerNewsItemBag.Add(result);

                PostNewResult(hackerNewsItemBag);
            });

        await Task.WhenAll(tasks);
    }

    private void PostNewResult(ConcurrentBag<HackerNewsItem> hackerNewsItemBag)
    {
        var processed = _bestStoriesToHackerNewsItemsProcessor.Process(hackerNewsItemBag.ToArray());

        _storyStore.ReplaceAll(processed);
    }
}
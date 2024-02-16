using BestOfHackerNews.Core.Interfaces;
using BestOfHackerNews.Core.Records;
using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Flurl.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using Polly;
using Polly.Retry;
using Serilog;

namespace BestOfHackerNews.Core.Implementations;

internal class BestStoriesCollector : ICollectBestStories, IDisposable
{
    private readonly IStoreBestStories _storyStore;
    private IDisposable _monitor;
    private readonly AsyncRetryPolicy _policy;
    private readonly string _bestStoriesUri;
    private readonly TimeSpan _period;

    public BestStoriesCollector(IStoreBestStories storyStore, IConfiguration config)
    {
        _storyStore = storyStore;

        var configurationSection = config.GetSection(Constants.Configuration.SectionName);
        var retryCount = configurationSection.GetValue<int>(Constants.Configuration.NewsApiRetryCount);
        var retryDelayMs = configurationSection.GetValue<int>(Constants.Configuration.NewsApiRetryDelayMs);
        var checkIntervalInSeconds = configurationSection.GetValue<int>(Constants.Configuration.NewsApiCheckIntervalInSeconds);
        _period = TimeSpan.FromSeconds(checkIntervalInSeconds);
        _bestStoriesUri = configurationSection.GetValue<string>(Constants.Configuration.BestStoriesUri);

        _policy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(retryCount, count =>
            {
                Log.Warning("Retry attempt {count}", count);
                return TimeSpan.FromMilliseconds(retryDelayMs);
            });
    }

    public async Task BeginCollection()
    {
        if (_monitor != null) throw new InvalidOperationException("Already initialised");

        Log.Information("Beginning monitoring of best stories every {@interval} from: {uri}", _period, _bestStoriesUri);

        await GetBestStories();

        _monitor = Observable
            .Interval(_period)
            .ObserveOn(NewThreadScheduler.Default)  //New thread protects from thread pool starvation caused by incoming requests
            .Select(_ => Observable.FromAsync(() => GetBestStories()))
            .Concat()
            .Subscribe(); 
    }

    private async Task GetBestStories(long _ = 0)
    {
        Log.Information("Checking for updates from: {uri}", _bestStoriesUri);

        var stories = await _bestStoriesUri.GetJsonAsync<int[]>();
    }

    public void Dispose()
    {
        _monitor.Dispose();
    }
}
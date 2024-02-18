using BestOfHackerNews.Core.Interfaces.Config;
using BestOfHackerNews.Core.Interfaces;
using BestOfHackerNews.Core.Records;
using Serilog;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace BestOfHackerNews.Core.Implementations;

/// <summary>
/// Monitors best story news items, calls the processor on those items and then updates the story store
/// </summary>
internal class BestStoriesCollector : ICollectBestStories, IDisposable
{
    private BestStoriesCollectorConfig _bestStoriesCollectorConfig;
    private readonly IBestStoriesCollectorConfigProvider _bestStoriesCollectorConfigProvider;
    private IDisposable _monitor;
    private readonly IProvideBestStoriesAsHackerNewsItems _bestStoriesProvider;

    public BestStoriesCollector(
        IProvideBestStoriesAsHackerNewsItems bestStoriesProvider,
        IBestStoriesCollectorConfigProvider bestStoriesCollectorConfigProvider)
    {
        _bestStoriesProvider = bestStoriesProvider;
        _bestStoriesCollectorConfigProvider = bestStoriesCollectorConfigProvider;
    }

    public async Task Start()
    {
        if (_monitor != null) throw new InvalidOperationException("Already initialised");

        _bestStoriesCollectorConfig = _bestStoriesCollectorConfigProvider.ReadConfig();

        await _bestStoriesProvider.GetBestStories();

        InitialiseMonitoring();
    }

    private void InitialiseMonitoring()
    {
        Log.Information("Beginning monitoring of best stories every {@interval}", _bestStoriesCollectorConfig.CheckIntervalInSeconds);
        _monitor = Observable
            .Interval(_bestStoriesCollectorConfig.CheckIntervalInSeconds)
            .ObserveOn(NewThreadScheduler.Default)  //New thread protects from thread pool starvation caused by incoming requests
            .Select(_ => Observable.FromAsync(() => _bestStoriesProvider.GetBestStories()))
            .Concat()
            .Subscribe();
    }

    public void Dispose()
    {
        _monitor.Dispose();
    }
}
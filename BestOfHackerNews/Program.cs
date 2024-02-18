using BestOfHackerNews.Core;
using BestOfHackerNews.Core.Extensions;
using Serilog;

//By creating the builder here, we allow hosts to influence the setup as required.
var builder = WebApplication.CreateBuilder(args);

BohnServiceHostConfigurator.ConfigureServices(builder);

builder.Host.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));

var app = builder.Build();

BohnServiceHostConfigurator.ConfigureApplication(app);

await app.BeginListeningToBohn();

Log.Information("Service started at {@time}", DateTime.UtcNow);

app.Run();

Log.Information("Service stopped at {@time}", DateTime.UtcNow);


using BestOfHackerNews.Core;
using Serilog;

//By creating the builder here, we allow hosts to influence the setup as required.
var builder = WebApplication.CreateBuilder(args);

BohnServiceHostConfigurator.ConfigureServices(builder);

builder.Host.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));

var app = builder.Build();

await BohnServiceHostConfigurator.ConfigureApplication(app);

Log.Information("Service started at {@time}", DateTime.UtcNow);

app.Run();

Log.Information("Service stopped at {@time}", DateTime.UtcNow);


using BestOfHackerNews.Core;

//By creating the builder here, we allow hosts to influence the setup as required.
var builder = WebApplication.CreateBuilder(args);

BohnServiceHostConfigurator.ConfigureServices(builder);

var app = builder.Build();

BohnServiceHostConfigurator.ConfigureApplication(app);

app.Run();



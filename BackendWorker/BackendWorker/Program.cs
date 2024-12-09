
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using BackendWorker.Controller.Coles;
using BackendWorker.Controller.Woolies;


var builder = FunctionsApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddSingleton<ProductSearchColesHelper>();
builder.Services.AddSingleton<ProductSearchWooliesHelper>();

builder.ConfigureFunctionsWebApplication();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

builder.Build().Run();

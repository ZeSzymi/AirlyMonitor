using AirlyInfrastructure.Contexts;
using AirlyInfrastructure.Repositories.Interfaces;
using AirlyInfrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using AlertsMonitor.Services.Interfaces;
using AlertsMonitor.Services;
using AirlyInfrastructure.Services.Interfaces;
using AirlyInfrastructure.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true);

builder.Services.AddScoped<IMeasurementRepository, MeasurementRepository>();
builder.Services.AddScoped<IInstallationsRepository, InstallationsRepository>();
builder.Services.AddScoped<IAlertDefinitionsRepository, AlertDefinitionsRepository>();
builder.Services.AddScoped<IAlertsRepository, AlertsRepository>();

builder.Services.AddScoped<IAlertsGeneratorService, AlertsGeneratorService>();
builder.Services.AddScoped<IAlertsService, AlertsService>();
builder.Services.AddScoped<IMeasurementsService, MeasurementsService>();
builder.Services.AddScoped<IAlertDefinitionService, AlertDefinitionsService>();
builder.Services.AddScoped<IAlertsMonitorService, AlertsMonitorService>();
builder.Services.AddHostedService<AlertsBackgroundService>();
builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("AirlyDb");
builder.Services.AddDbContext<AirlyDbContext>(x => x.UseSqlServer(connectionString));

builder.Host.UseSerilog((context, lc) => lc.ReadFrom.Configuration(builder.Configuration).Enrich.FromLogContext());

var app = builder.Build();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.Run();
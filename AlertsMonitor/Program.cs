using AirlyInfrastructure.Contexts;
using AirlyInfrastructure.Repositories.Interfaces;
using AirlyInfrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using AlertsMonitor.Services.Interfaces;
using AlertsMonitor.Services;
using AirlyInfrastructure.Services.Interfaces;
using AirlyInfrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IMeasurementRepository, MeasurementRepository>();
builder.Services.AddScoped<IInstallationRepository, InstallationsRepository>();
builder.Services.AddScoped<IAlertDefinitionsRepository, AlertDefinitionsRepository>();
builder.Services.AddScoped<IAlertsRepository, AlertsRepository>();

builder.Services.AddScoped<IAlertsGeneratorService, AlertsGeneratorService>();
builder.Services.AddScoped<IAlertsService, AlertsService>();
builder.Services.AddScoped<IMeasurementsService, MeasurementsService>();
builder.Services.AddScoped<IAlertDefinitionService, AlertDefinitionsService>();
builder.Services.AddHostedService<AlertsBackgroundService>();

var connectionString = builder.Configuration.GetConnectionString("AirlyDb");
builder.Services.AddDbContext<AirlyDbContext>(x => x.UseSqlServer(connectionString));

var app = builder.Build();

app.Run();
using AirlyInfrastructure.Contexts;
using AirlyInfrastructure.Repositories.Interfaces;
using AirlyInfrastructure.Repositories;
using MeasurementsSimulator.Services.Interfaces;
using MeasurementsSimulator.Services;
using Microsoft.EntityFrameworkCore;
using AirlyInfrastructure.Services.Interfaces;
using AirlyInfrastructure.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true);

builder.Services.AddScoped<IMeasurementRepository, MeasurementRepository>();
builder.Services.AddScoped<IAlertDefinitionsRepository, AlertDefinitionsRepository>();
builder.Services.AddScoped<IInstallationsRepository, InstallationsRepository>();

builder.Services.AddScoped<IMeasurementsService, MeasurementsService>();
builder.Services.AddScoped<IMeasurementGenerationService, MeasurementGenerationService>();
builder.Services.AddScoped<IMeasurementSimulatorService, MeasurementSimulatorService>();
builder.Services.AddHostedService<MeasurementsBackgroundService>();

builder.Services.AddControllers();

builder.Host.UseSerilog((context, lc) => lc.ReadFrom.Configuration(builder.Configuration).Enrich.FromLogContext());

var connectionString = builder.Configuration.GetConnectionString("AirlyDb");
builder.Services.AddDbContext<AirlyDbContext>(x => x.UseSqlServer(connectionString));

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

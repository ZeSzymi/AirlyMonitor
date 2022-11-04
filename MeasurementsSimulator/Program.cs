using AirlyInfrastructure.Contexts;
using AirlyInfrastructure.Repositories.Interfaces;
using AirlyInfrastructure.Repositories;
using MeasurementsSimulator.Services.Interfaces;
using MeasurementsSimulator.Services;
using Microsoft.EntityFrameworkCore;
using AirlyInfrastructure.Services.Interfaces;
using AirlyInfrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IMeasurementRepository, MeasurementRepository>();
builder.Services.AddScoped<IInstallationRepository, InstallationsRepository>();
builder.Services.AddScoped<IAlertDefinitionsRepository, AlertDefinitionsRepository>();

builder.Services.AddScoped<IMeasurementsService, MeasurementsService>();
builder.Services.AddScoped<IMeasurementGenerationService, MeasurementGenerationService>();
builder.Services.AddHostedService<MeasurementsBackgroundService>();

var connectionString = builder.Configuration.GetConnectionString("AirlyDb");
builder.Services.AddDbContext<AirlyDbContext>(x => x.UseSqlServer(connectionString));

var app = builder.Build();

app.Run();

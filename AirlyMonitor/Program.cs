using AirlyInfrastructure.Contexts;
using AirlyInfrastructure.Repositories;
using AirlyInfrastructure.Repositories.Interfaces;
using AirlyInfrastructure.Services;
using AirlyInfrastructure.Services.Interfaces;
using AirlyMonitor.Models.Configuration;
using AirlyMonitor.Services;
using AirlyMonitor.Services.Interface;
using AirlyMonitor.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true);

builder.Host.UseSerilog((context, lc) => lc.ReadFrom.Configuration(builder.Configuration).Enrich.FromLogContext());


var connectionString = builder.Configuration.GetConnectionString("AirlyDb");

builder.Services.AddDbContext<AirlyDbContext>(x => x.UseSqlServer(connectionString));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

builder.Services.AddScoped<IMeasurementRepository, MeasurementRepository>();
builder.Services.AddScoped<IInstallationRepository, InstallationsRepository>();
builder.Services.AddScoped<IAlertDefinitionsRepository, AlertDefinitionsRepository>();
builder.Services.AddScoped<IAlertsRepository, AlertsRepository>();

builder.Services.AddScoped<IAlertsService, AlertsService>();
builder.Services.AddScoped<IAlertDefinitionsService, AirlyMonitor.Services.AlertDefinitionsService>();
builder.Services.AddScoped<IAirlyApiService, AirlyApiService>();
builder.Services.AddScoped<IHttpService, HttpService>();

builder.Services.Configure<AirlyApiOptions>(builder.Configuration.GetSection("AirlyApi"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();


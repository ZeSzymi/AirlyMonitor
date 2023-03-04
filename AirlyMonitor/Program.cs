using AirlyInfrastructure.Contexts;
using AirlyInfrastructure.Extentions;
using AirlyInfrastructure.Models.Options;
using AirlyInfrastructure.Repositories;
using AirlyInfrastructure.Repositories.Interfaces;
using AirlyInfrastructure.Services;
using AirlyInfrastructure.Services.Interfaces;
using AirlyMonitor.Extensions;
using AirlyMonitor.Middleware;
using AirlyMonitor.Models.Configuration;
using AirlyMonitor.Services;
using AirlyMonitor.Services.Interface;
using AirlyMonitor.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true);

builder.Host.UseSerilog((context, lc) => lc.WriteTo.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs/.log"), rollingInterval: RollingInterval.Day));

var connectionString = builder.Configuration.GetConnectionString("AirlyDb");
builder.Services.AddDbContext<AirlyDbContext>(x => x.UseSqlServer(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger(builder.Configuration);
builder.Services.AddHttpClient();

builder.Services.AddScoped<IMeasurementRepository, MeasurementRepository>();
builder.Services.AddScoped<IInstallationsRepository, InstallationsRepository>();
builder.Services.AddScoped<IAlertDefinitionsRepository, AlertDefinitionsRepository>();
builder.Services.AddScoped<IAlertsRepository, AlertsRepository>();

builder.Services.AddScoped<IMeasurementsService, MeasurementsService>();
builder.Services.AddScoped<IAlertsService, AlertsService>();
builder.Services.AddScoped<IAlertDefinitionsService, AirlyMonitor.Services.AlertDefinitionsService>();
builder.Services.AddScoped<IInstallationsService, InstallationsService>();
builder.Services.AddScoped<IAirlyApiService, AirlyApiService>();
builder.Services.AddScoped<IPushNotificationsHttpService, PushNotificationsHttpService>();
builder.Services.AddScoped<IHttpService, HttpService>();
builder.Services.AddHttpContextAccessor();

builder.Services.Configure<AirlyApiOptions>(builder.Configuration.GetSection("AirlyApi"));

var rabbitConfiguration = builder.Configuration.GetSection("RabbitConfiguration").Get<RabbitConfigurationOptions>();
builder.Services.RegisterMassTransit(rabbitConfiguration);


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            var cors = builder.Configuration.GetSection("CORS:origins").Get<string[]>();
            policy
                .WithOrigins(cors)
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Services.AddAuthentication("Bearer")
           .AddJwtBearer("Bearer", options =>
           {
               options.Authority = builder.Configuration.GetSection("Auth:Authority").Get<string>();
               options.RequireHttpsMetadata = false;
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateAudience = false
               };
           });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "api");
    });
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseSwagger();
app.AddSwaggerUI(builder.Configuration);

app.UseMiddleware<ExceptionMiddleware>();
app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();


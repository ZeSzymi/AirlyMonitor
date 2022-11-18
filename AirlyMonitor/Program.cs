using AirlyInfrastructure.Contexts;
using AirlyInfrastructure.Repositories;
using AirlyInfrastructure.Repositories.Interfaces;
using AirlyInfrastructure.Services;
using AirlyInfrastructure.Services.Interfaces;
using AirlyMonitor.Models.Configuration;
using AirlyMonitor.Services;
using AirlyMonitor.Services.Interface;
using AirlyMonitor.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true);

builder.Host.UseSerilog((context, lc) => lc.ReadFrom.Configuration(builder.Configuration).Enrich.FromLogContext());

var connectionString = builder.Configuration.GetConnectionString("AirlyDb");

builder.Services.AddDbContext<AirlyDbContext>(x => x.UseSqlServer(connectionString));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    var scheme = new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri(builder.Configuration.GetSection("Auth:Swagger:AuthorizationUrl").Get<string>()),
                TokenUrl = new Uri(builder.Configuration.GetSection("Auth:Swagger:TokenUrl").Get<string>())
            }
        },
        Type = SecuritySchemeType.OAuth2
    };

    options.AddSecurityDefinition("OAuth", scheme);

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Id = "OAuth", Type = ReferenceType.SecurityScheme }
            },
            new List<string> { }
        }
    });
});

builder.Services.AddHttpClient();

builder.Services.AddScoped<IMeasurementRepository, MeasurementRepository>();
builder.Services.AddScoped<IInstallationRepository, InstallationsRepository>();
builder.Services.AddScoped<IAlertDefinitionsRepository, AlertDefinitionsRepository>();
builder.Services.AddScoped<IAlertsRepository, AlertsRepository>();
builder.Services.AddScoped<IMeasurementsService, MeasurementsService>();

builder.Services.AddScoped<IAlertsService, AlertsService>();
builder.Services.AddScoped<IAlertDefinitionsService, AirlyMonitor.Services.AlertDefinitionsService>();
builder.Services.AddScoped<IAirlyApiService, AirlyApiService>();
builder.Services.AddScoped<IHttpService, HttpService>();

builder.Services.Configure<AirlyApiOptions>(builder.Configuration.GetSection("AirlyApi"));

builder.Services.AddAuthentication("Bearer")
           .AddJwtBearer("Bearer", options =>
           {
               options.Authority = builder.Configuration.GetSection("Auth:Authority").Get<string>();

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
    options.OAuthClientId("swagger");
    options.OAuthClientSecret("swagger-secret-4");
    options.OAuthScopes("api");
    options.OAuthUsePkce();
    options.EnablePersistAuthorization();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();


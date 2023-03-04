using MassTransit;
using PushNotificationsService.Consumers;
using Serilog;
using PushNotificationsService.Services;
using PushNotificationsService.Services.Interfaces;
using PushNotificationsService.Options;
using AirlyInfrastructure.Models.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5013");

builder.Configuration.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true);
builder.Host.UseSerilog((context, lc) => lc.WriteTo.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs/.log"), rollingInterval: RollingInterval.Day));

builder.Services.AddSingleton<IFirebaseCloudMessageService, FireBaseCloudMessagesService>();
builder.Services.AddScoped<IEmailNotificationService, EmailNotificationService>();
builder.Services.AddHostedService<TokenValidityBackgroundService>();

builder.Services.AddControllers();

builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("Email"));

var rabbitConfiguration = builder.Configuration.GetSection("RabbitConfiguration").Get<RabbitConfigurationOptions>();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumers(typeof(PushNotificationMessagesConsumer).Assembly);
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitConfiguration.Address, "/", h =>
        {
            h.Username(rabbitConfiguration.UserName);
            h.Password(rabbitConfiguration.Password);
        });

        cfg.ReceiveEndpoint("PushNotificationMessage", p =>
        {
            p.ConfigureConsumer<PushNotificationMessagesConsumer>(context);
        });

        cfg.ConfigureEndpoints(context);
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

app.UseRouting();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
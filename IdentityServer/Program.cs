using IdentityServer.Contexts;
using IdentityServer.Extensions;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5010");

var connectionString = builder.Configuration.GetConnectionString("AirlyDb");

builder.Host.UseSerilog((context, lc) => lc.WriteTo.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs/.log"), rollingInterval: RollingInterval.Day));

builder.Services.AddDbContext<IdentityDbContext>(options =>
{
    options.UseSqlServer(connectionString);
    options.UseOpenIddict();
});

builder.Services.AddControllers();
builder.Services.AddIdentity();
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

builder.Services.AddOpenIddictOptions();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.MapFallbackToFile("index.html");

app.UseRouting();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();


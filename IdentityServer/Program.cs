using IdentityServer.Contexts;
using IdentityServer.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("AirlyDb");

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

app.UseHttpsRedirection();
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


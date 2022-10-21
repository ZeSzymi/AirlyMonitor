using AirlyInfrastructure.Contexts;
using AirlyInfrastructure.Repositories;
using AirlyInfrastructure.Repositories.Interfaces;
using AirlyMonitor.Models.Configuration;
using AirlyMonitor.Services;
using AirlyMonitor.Services.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("AirlyDb");

builder.Services.AddDbContext<AirlyDbContext>(x => x.UseSqlServer(connectionString));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

builder.Services.AddScoped<IMeasurementRepository, MeasurementRepository>();
builder.Services.AddScoped<IHttpService, HttpService>();

builder.Services.Configure<AirlyApiOptions>(builder.Configuration.GetSection("AirlyApi"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseSwagger();
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();


using AirlyInfrastructure.Contexts;

var builder = Microsoft. WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("AirlyDb");
builder.Services.AddDbContext<AirlyDbContext>(x => x.UseSqlServer(connectionString));

var app = builder.Build();

Console.WriteLine("Hello, World!");

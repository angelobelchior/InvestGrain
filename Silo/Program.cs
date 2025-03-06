using InvestGrain.ServiceDefaults;
using InvestGrain.Silo.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
builder.AddKeyedRedisClient("redis");
builder.Host.UseOrleans(orleans => { orleans.UseDashboard(); });
var app = builder.Build();
app.MapGet("/", () => "OK");
app.Run();
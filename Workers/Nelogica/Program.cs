using InvestGrain.ServiceDefaults;
using InvestGrain.Worker.Nelogica;
using InvestGrain.Worker.Nelogica.Repositories;

var builder = Host.CreateApplicationBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddSingleton<IOrdersRepository, OrdersRepository>();
builder.AddKeyedRedisClient("redis");
builder.UseOrleansClient();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();

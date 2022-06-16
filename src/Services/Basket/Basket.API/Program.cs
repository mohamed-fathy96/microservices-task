using Basket.API.Repositories;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(Program));

// Register BasketRepository

builder.Services.AddScoped<IBasketRepository, BasketRepository>();

// Configure Redis, and inject IDistributedCache

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration
    .GetValue<string>("CacheSettings:ConnectionString");
});

// Configure MassTransit with RabbitMQ, to produce Basket Checkout event

builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
    });
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapControllers();

app.Run();

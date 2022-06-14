using Basket.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Register BasketRepository

builder.Services.AddScoped<IBasketRepository, BasketRepository>();

// Configure Redis, and inject IDistributedCache

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration
    .GetValue<string>("CacheSettings:ConnectionString");
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

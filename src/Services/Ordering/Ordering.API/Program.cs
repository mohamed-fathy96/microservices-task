using Eventbus.Messages.Common;
using MassTransit;
using Ordering.API.EventBusConsumer;
using Ordering.API.Extensions;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add AutoMapper

builder.Services.AddAutoMapper(typeof(Program));

// Add Basket Consumer

builder.Services.AddScoped<BasketCheckoutConsumer>();

// Add Application and Infrastructure Services

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Configure MassTransit and RabbitMQ to consume checkout event

builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<BasketCheckoutConsumer>();

    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);

        cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c =>
        {
            c.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
        });
    });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OrderContext>();
    var services = app.Services;
    app.MigrateDatabase<OrderContext>((dbContext, services) =>
    {
        var logger = services.GetService<ILogger<OrderContextSeed>>();
        OrderContextSeed.SeedAsync(dbContext, logger).Wait();
    }, scope);
}

app.Run();

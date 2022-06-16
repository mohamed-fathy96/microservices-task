using Microsoft.Extensions.Logging;
using Ordering.Domain.Models;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetDummyOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrderContext).Name);
            }
        }

        private static IEnumerable<Order> GetDummyOrders()
        {
            return new List<Order>
            {
                new Order() {UserName = "Fathy", FirstName = "Mohamed", LastName = "Fathy", EmailAddress = "hahatakken2007@gmail.com", AddressLine = "Asdf", Country = "Egypt", TotalPrice = 500 }
            };
        }
    }
}

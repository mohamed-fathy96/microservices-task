using Basket.API.Models;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache redisCache;
        public BasketRepository(IDistributedCache redisCache)
        {
            this.redisCache = redisCache;
        }
        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var basket = await redisCache.GetStringAsync(userName);

            if (string.IsNullOrEmpty(basket))
                return null;

            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }
        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            // Set new basket as value, with username as key

            await redisCache.SetStringAsync(basket.UserName, 
                JsonConvert.SerializeObject(basket));

            // Return updated basket

            return await GetBasket(basket.UserName);
        }
        public async Task DeleteBasket(string userName)
        {
            await redisCache.RemoveAsync(userName);
        }
    }
}

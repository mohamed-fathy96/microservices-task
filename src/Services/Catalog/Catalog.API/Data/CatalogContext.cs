using Catalog.API.Models;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IConfiguration config)
        {
            var client = new MongoClient(config["DatabaseSettings:ConnectionString"]);
            var database = client.GetDatabase(config["DatabaseSettings:DatabaseName"]);

            Products = database.GetCollection<Product>(config["DatabaseSettings:CollectionName"]);
            CatalogContextSeed.SeedData(Products);
        }
        public IMongoCollection<Product> Products { get; }

    }
}

using Catalog.API.Models;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository repository;
        private readonly ILogger<CatalogController> logger;
        public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        // Use annotations to specify HTTP return type

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await repository.GetProducts();
            return Ok(products);
        }

        // BSON Id is 24 characters in length

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var product = await repository.GetProductById(id);
            if (product == null)
            {
                logger.LogError($"Product with Id: {id} was not found.");
                return NotFound();
            }

            return Ok(product);
        }
    }
}

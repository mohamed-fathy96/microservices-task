using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Catalog.API.Repositories;
using Catalog.API.Models;
using Catalog.API.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.UnitTests.Application
{
    public class CatalogControllerTest
    {
        private readonly Mock<IProductRepository> mockProductRepository;
        public CatalogControllerTest()
        {
            mockProductRepository = new();
            mockProductRepository.Setup(o => o.GetProducts()).ReturnsAsync(GetFakeProducts());
        }

        [Fact]
        public async Task GetProducts_ReturnsCorrectListOfProducts()
        {
            // Arrange
            
            var catalogController = new CatalogController(mockProductRepository.Object, null);

            // Act

            var actionResult = await catalogController.GetProducts();

            // Assert

            Assert.IsType<ActionResult<IEnumerable<Product>>>(actionResult);

            var result = actionResult.Result as OkObjectResult;

            Assert.NotNull(result);

            var actualProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(result.Value);

            var expectedProducts = await mockProductRepository.Object.GetProducts();

            Assert.Equal(expectedProducts, actualProducts);
        }
        
        [Fact]
        public async Task GetProductById_ProvideExistingProductId_ReturnsCorrectProduct()
        {
            // Arrange

            var product = new Product()
            {
                Id = "602d2149e773f2a3990b47f5",
                Name = "Product A",
                Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                Image = "product-1.png",
                Price = 950.00M
            };

            mockProductRepository.Setup
                (o => o.GetProductById("602d2149e773f2a3990b47f5")).ReturnsAsync(product);

            var catalogController = new CatalogController(mockProductRepository.Object, null);

            // Act

            var actionResult = await catalogController.GetProductById("602d2149e773f2a3990b47f5");

            // Assert

            Assert.IsType<ActionResult<Product>>(actionResult);

            var result = actionResult.Result as OkObjectResult;

            Assert.NotNull(result);

            var actualProduct = Assert.IsAssignableFrom<Product>(result.Value);

            Assert.Equal(product, actualProduct);

        }

        [Fact]
        public async Task GetProductById_ProvideNonExistingProductId_ReturnsNotFound()
        {
            // Arrange

            mockProductRepository.Setup
                (o => o.GetProductById("abc")).ReturnsAsync((Product)null);
            var catalogController = new CatalogController(mockProductRepository.Object, null);

            // Act

            var actionResult = await catalogController.GetProductById("abc");

            // Assert

            Assert.IsType<ActionResult<Product>>(actionResult);

            Assert.True(actionResult.Result is NotFoundResult);

        }
        private IEnumerable<Product> GetFakeProducts()
        {
            var products = new List<Product>()
            {
                new Product()
                {
                    Id = "602d2149e773f2a3990b47f5",
                    Name = "Product A",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                    Image = "product-1.png",
                    Price = 950.00M
                },
                new Product()
                {
                    Id = "602d2149e773f2a3990b47f6",
                    Name = "Product B",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                    Image = "product-2.png",
                    Price = 840.00M
                },
                new Product()
                {
                    Id = "602d2149e773f2a3990b47f7",
                    Name = "Product C",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                    Image = "product-3.png",
                    Price = 650.00M
                },
                new Product()
                {
                    Id = "602d2149e773f2a3990b47f8",
                    Name = "Product D",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                    Image = "product-4.png",
                    Price = 470.00M
                },
                new Product()
                {
                    Id = "602d2149e773f2a3990b47f9",
                    Name = "Product E",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                    Image = "product-5.png",
                    Price = 380.00M
                },
                new Product()
                {
                    Id = "602d2149e773f2a3990b47fa",
                    Name = "Product F",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                    Image = "product-6.png",
                    Price = 240.00M
                }
        };
            return products;
        }
    }
}

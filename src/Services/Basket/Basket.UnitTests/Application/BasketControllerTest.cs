using Basket.API.Controllers;
using Basket.API.Models;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Basket.UnitTests.Application
{
    public class BasketControllerTest
    {
        private readonly Mock<IBasketRepository> mockBasketRepository;
        public BasketControllerTest()
        {
            this.mockBasketRepository = new();
        }

        [Fact]
        public async Task GetBasket_ProvideExistingUserName_ReturnsUserBasket()
        {
            // Arrange

            var basket = new ShoppingCart("fathy");

            mockBasketRepository.Setup(o => o.GetBasket("fathy")).ReturnsAsync(basket);

            var basketController = new BasketController(mockBasketRepository.Object, null, null);

            // Act

            var actionResult = await basketController.GetBasket("fathy");

            // Assert

            Assert.IsType<ActionResult<ShoppingCart>>(actionResult);

            var result = actionResult.Result as OkObjectResult;

            Assert.NotNull(result);

            var actualBasket = Assert.IsAssignableFrom<ShoppingCart>(result.Value);

            Assert.True(basket.UserName == actualBasket.UserName);

            Assert.Equal(basket, actualBasket);
        }

        [Fact]
        public async Task UpdateBasket_ProvideBasketToUpdate_ReturnsCorrectUpdatedBasket()
        {
            // Arrange

            var oldBasket = new ShoppingCart("fathy");

            var newBasket = new ShoppingCart("fathy");
            newBasket.Items.Add(new ShoppingCartItem()
            {
                Quantity = 1,
                ProductId = "x",
                ProductName = "abc",
                Price = 500
            });
            mockBasketRepository.Setup(o => o.UpdateBasket(oldBasket)).ReturnsAsync(newBasket);

            var basketController = new BasketController(mockBasketRepository.Object, null, null);

            // Act
            
            var actionResult = await basketController.UpdateBasket(oldBasket);

            // Assert

            Assert.IsType<ActionResult<ShoppingCart>>(actionResult);

            var result = actionResult.Result as OkObjectResult;

            Assert.NotNull(result);

            var actualBasket = Assert.IsAssignableFrom<ShoppingCart>(result.Value);

            Assert.Equal(newBasket, actualBasket);
        }
    }
}

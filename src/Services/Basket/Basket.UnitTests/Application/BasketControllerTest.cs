using AutoMapper;
using Basket.API.Controllers;
using Basket.API.Mapper;
using Basket.API.Models;
using Basket.API.Repositories;
using Eventbus.Messages.Events;
using MassTransit;
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
        private readonly Mock<IPublishEndpoint> mockPublishEndPoint;
        private readonly IMapper mapper;
        public BasketControllerTest()
        {
            this.mockBasketRepository = new();
            this.mockPublishEndPoint = new();
            var mapperConfig = new MapperConfiguration(c => {
                c.AddProfile<BasketProfile>();
            });
            this.mapper = mapperConfig.CreateMapper();
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

            Assert.True(newBasket.UserName == actualBasket.UserName);

            Assert.True(newBasket.Items.Count == 1);
        }

        [Fact]
        public async Task Checkout_ProvideCorrectBasketToCheckOut_PublishesEventAndReturnsAccepted()
        {
            // Arrange

            var basketCheckout = new BasketCheckout()
            {
                UserName = "fathy",
                TotalPrice = 0,
            };

            var basketCheckOutEvent = new BasketCheckoutEvent()
            {
                UserName = "fathy",
                TotalPrice = 0
            };

            var basket = new ShoppingCart("fathy");

            mockBasketRepository.Setup(o => o.GetBasket("fathy")).ReturnsAsync(basket);
            mockPublishEndPoint.Setup(o => o.Publish(It.IsAny<BasketCheckoutEvent>(), default));

            var basketController =
                new BasketController(mockBasketRepository.Object,
                mapper, mockPublishEndPoint.Object);

            // Act

            var actionResult = await basketController.Checkout(basketCheckout);

            // Assert

            mockPublishEndPoint.Verify(o => o.Publish(It.IsAny<BasketCheckoutEvent>(), default), Times.Once);
            
            Assert.True(actionResult is AcceptedResult);
            
            Assert.NotNull(actionResult);
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ordering.API.Controllers;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using Ordering.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ordering.UnitTests.Application
{
    public class OrdersControllerTest
    {
        private readonly Mock<IMediator> mockMediator;
        public OrdersControllerTest()
        {
            mockMediator = new();
        }

        [Fact]
        public async Task GetOrdersByUserName_ProvideCorrectUserName_ReturnsOkWithOrders()
        {
            // Arrange

            var fakeOrders = GetFakeUserOrders();

            mockMediator.Setup(o
                => o.Send(It.IsAny<GetOrdersListQuery>(), default)).ReturnsAsync(fakeOrders);

            var ordersController = new OrdersController(mockMediator.Object);

            // Act

            var actionResult = await ordersController.GetOrdersByUserName("fathy");

            // Assert

            Assert.IsType<ActionResult<IEnumerable<OrderDTO>>>(actionResult);

            var result = actionResult.Result as OkObjectResult;

            Assert.NotNull(result);

            var actualOrders = Assert.IsAssignableFrom<IEnumerable<OrderDTO>>(result.Value);

            Assert.True(fakeOrders.Count == actualOrders.Count());

        }

        [Fact]
        public async Task UpdateOrder_ProvideUpdateCommand_ReturnsNoContentAndSendsCommand()
        {
            // Arrange

            mockMediator.Setup(o => o.Send(It.IsAny<UpdateOrderCommand>(), default)).ReturnsAsync(Unit.Value);
            
            var ordersController = new OrdersController(mockMediator.Object);

            var orderCommand = new UpdateOrderCommand();

            // Act

            var actionResult = await ordersController.UpdateOrder(orderCommand);

            // Assert

            Assert.True(actionResult is NoContentResult);

            mockMediator.Verify(o => o.Send(It.IsAny<UpdateOrderCommand>(), default), Times.Once);
        }

        [Fact]
        public async Task DeleteOrder_ProvideOrderId_ReturnsNoContentAndSendsCommand()
        {
            // Arrange

            mockMediator.Setup(o => o.Send(It.IsAny<DeleteOrderCommand>(), default)).ReturnsAsync(Unit.Value);

            var ordersController = new OrdersController(mockMediator.Object);

            // Act

            var actionResult = await ordersController.DeleteOrder(1);

            // Assert

            Assert.True(actionResult is NoContentResult);

            mockMediator.Verify(o => o.Send(It.IsAny<DeleteOrderCommand>(), default), Times.Once);
        }
        private List<OrderDTO> GetFakeUserOrders()
        {
            var orders = new List<OrderDTO>()
            {
                new()
                {
                    UserName = "fathy",
                    TotalPrice = 1000
                },
                new()
                {
                    UserName = "fathy",
                    TotalPrice = 2000
                }
            };

            return orders;
        }
    }
}

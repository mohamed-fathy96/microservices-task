using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using AutoMapper;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Mappings;
using Ordering.Domain.Models;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;

namespace Ordering.UnitTests.Application
{
    public class GetOrdersListQueryHandlerTest
    {
        private readonly IMapper mapper;
        private readonly Mock<IOrderRepository> mockOrderRepository;
        public GetOrdersListQueryHandlerTest()
        {
            this.mockOrderRepository = new();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task Handle_ProvideExistingUserName_ReturnsListOfUserOrders()
        {
            // Arrange
            var fakeOrders = GetFakeUserOrders();
            mockOrderRepository.Setup(o =>
                o.GetOrdersByUserName("fathy")).ReturnsAsync(fakeOrders);
            var handler = new GetOrdersListQueryHandler(mockOrderRepository.Object, mapper);
            var request = new GetOrdersListQuery("fathy");

            // Act

            var result = await handler.Handle(request, default);

            // Assert

            Assert.IsType<List<OrderDTO>>(result);
            Assert.True(result.Count == fakeOrders.Count());

        }
        private IEnumerable<Order> GetFakeUserOrders()
        {
            var orders = new List<Order>()
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

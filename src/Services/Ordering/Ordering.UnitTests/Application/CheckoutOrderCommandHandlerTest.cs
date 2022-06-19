using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Ordering.Application.Contracts.Persistence;
using AutoMapper;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Microsoft.Extensions.Logging;
using Ordering.Application.Mappings;
using Ordering.Domain.Models;
using Ordering.Application.Models;

namespace Ordering.UnitTests.Application
{
    public class CheckoutOrderCommandHandlerTest
    {
        private readonly Mock<IOrderRepository> mockOrderRepository;
        private readonly IMapper mapper;
        private readonly Mock<IEmailService> mockMailService;
        private readonly Mock<ILogger<CheckoutOrderCommandHandler>> mockLogger;

        public CheckoutOrderCommandHandlerTest()
        {
            this.mockOrderRepository = new();
            this.mockMailService = new();
            this.mockLogger = new();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task Handle_ProvideValidRequest_ReturnsNewOrderId()
        {
            // Arrange

            var request = new CheckoutOrderCommand()
            {
                UserName = "fathy"
            };

            var fakeOrder = new Order()
            {
                UserName = "fathy"
            };

            mockOrderRepository.Setup(o => o.AddAsync(It.IsAny<Order>())).ReturnsAsync(fakeOrder);
            mockMailService.Setup(o => o.SendEmail(It.IsAny<Email>()));            
            var handler = new CheckoutOrderCommandHandler(mockOrderRepository.Object,
                mapper,mockMailService.Object, mockLogger.Object);

            // Act

            var result = await handler.Handle(request, default);

            // Assert

            mockMailService.Verify(o => o.SendEmail(It.IsAny<Email>()), Times.Once);

            mockOrderRepository.Verify(o => o.AddAsync(It.IsAny<Order>()), Times.Once);

            Assert.True(fakeOrder.Id == result);

        }
    }
}

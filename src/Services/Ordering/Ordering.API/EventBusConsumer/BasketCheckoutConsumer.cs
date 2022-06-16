using AutoMapper;
using Eventbus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;

namespace Ordering.API.EventBusConsumer
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMapper mapper;
        private readonly IMediator mediator;
        private readonly ILogger<BasketCheckoutConsumer> logger;
        public BasketCheckoutConsumer(IMapper mapper, 
            IMediator mediator, ILogger<BasketCheckoutConsumer> logger)
        {
            this.mapper = mapper;
            this.mediator = mediator;
            this.logger = logger;
        }
        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var command = mapper.Map<CheckoutOrderCommand>(context.Message);
            var result = await mediator.Send(command);

            logger.LogInformation("Consumed BasketCheckoutEvent success, order with Id {result} created.", result);
        }
    }
}

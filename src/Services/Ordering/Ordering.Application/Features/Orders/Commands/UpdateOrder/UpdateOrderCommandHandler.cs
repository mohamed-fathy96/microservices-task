using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;
        private readonly ILogger<CheckoutOrderCommandHandler> logger;
        public UpdateOrderCommandHandler(IOrderRepository orderRepository, 
            IMapper mapper, ILogger<CheckoutOrderCommandHandler> logger)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        // Unit means the method returns nothing 
        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var oldOrder = await orderRepository.GetByIdAsync(request.Id);
            if(oldOrder == null)
            {
                throw new NotFoundException(nameof(Order), request.Id);
            }

            // Map the request object to Order entity, then update it using repository

            mapper.Map(request, oldOrder, typeof(UpdateOrderCommand), typeof(Order));

            await orderRepository.UpdateAsync(oldOrder);
            logger.LogInformation($"Order with Id: {oldOrder.Id} is successfully updated.");

            return Unit.Value;
        }
    }
}

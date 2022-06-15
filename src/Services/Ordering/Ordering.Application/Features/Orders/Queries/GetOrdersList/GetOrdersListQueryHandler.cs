using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    public class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery, List<OrderDTO>>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;
        public GetOrdersListQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
        }
        public async Task<List<OrderDTO>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
        {
            var orders = await orderRepository.GetOrdersByUserName(request.UserName);
            return mapper.Map<List<OrderDTO>>(orders);
        }
    }
}

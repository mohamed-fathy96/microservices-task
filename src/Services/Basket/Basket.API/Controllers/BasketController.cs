using AutoMapper;
using Basket.API.Models;
using Basket.API.Repositories;
using Eventbus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository repository;
        private readonly IMapper mapper;
        private readonly IPublishEndpoint publishEndpoint;
        public BasketController(IBasketRepository repository, 
            IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.publishEndpoint = publishEndpoint;
        }

        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await repository.GetBasket(userName);

            // If basket is not null, return it, otherwise create new cart and return it
            return Ok(basket ?? new ShoppingCart(userName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            return Ok(await repository.UpdateBasket(basket));
        }

        [HttpDelete("{userName}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await repository.DeleteBasket(userName);
            return Ok();
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            // 1- Get existing basket for user

            var basket = await repository.GetBasket(basketCheckout.UserName);
            if (basket == null)
            {
                return BadRequest();
            }
                     
            // 2- Send checkout event to RabbitMQ

            var eventMessage = mapper.Map<BasketCheckoutEvent>(basketCheckout);

            // Set totalPrice on checkout Event

            eventMessage.TotalPrice = basket.TotalPrice;
            await publishEndpoint.Publish(eventMessage);

            // 3- Remove basket from Redis

            await repository.DeleteBasket(basket.UserName);

            return Accepted();
        }
    }
}

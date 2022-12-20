using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Timelapse.Models;
using Timelapse.Logic;

namespace Timelapse.Api.Controllers
{
    [Route("[controller]")]
    public class OrderApi : Controller
    {
        private readonly IOrderLogic _ibus;
        private readonly ILogger<OrderApi> _logger;

        public OrderApi(IOrderLogic ibus, ILogger<OrderApi> logger)
        {
            _ibus = ibus;
            _logger = logger;
        }

        [HttpPost("api/addOrder")]
        public async Task<ActionResult<OrderWrapper>> PostOrder([FromBody] OrderWrapper order)
        {
            try
            {
                await _ibus.PostOrder(order);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Problem();
            }

            return Ok();
        }

        [HttpGet("api/customer/{id}/orders")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders([FromRoute] string email)
        {
            IEnumerable<Order> orders;

            try
            {
                orders = await _ibus.GetOrders(email);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Problem();

            }

            return orders.ToList();
        }

        [HttpGet("api/customer/{userId}/orders/{orderId}")]
        public async Task<ActionResult<OrderWrapper>> GetOrderById([FromRoute] Guid userId, Guid orderId)
        {
            OrderWrapper order;

            try
            {
                order = await _ibus.GetOrderById(userId, orderId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Problem();

            }

            return order;
        }
    }
}
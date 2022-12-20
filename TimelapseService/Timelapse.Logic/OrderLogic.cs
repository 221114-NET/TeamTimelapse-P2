using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timelapse.Models;
using Timelapse.Repo;

namespace Timelapse.Logic
{
    public class OrderLogic : IOrderLogic
    {
        private readonly IOrderRepo _repo;

        public OrderLogic(IOrderRepo repo)
        {
            _repo = repo;
        }


        public async Task PostOrder(OrderWrapper order)
        {
            await _repo.PostOrder(order);
        }

        public async Task<IEnumerable<Order>> GetOrders(string email)
        {
            return await _repo.GetOrders(email);
        }

        public async Task<OrderWrapper> GetOrderById(Guid userId, Guid orderId)
        {
            return await _repo.GetOrderById(userId, orderId);
        }
    }
}
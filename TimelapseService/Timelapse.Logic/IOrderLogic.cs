using Timelapse.Models;

namespace Timelapse.Logic
{
    public interface IOrderLogic
    {
        Task PostOrder(OrderWrapper order);
        Task<IEnumerable<Order>> GetOrders(string email);
        Task<OrderWrapper> GetOrderById(Guid userId, Guid orderId);
    }
}
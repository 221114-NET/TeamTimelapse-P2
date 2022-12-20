using Timelapse.Models;

namespace Timelapse.Repo
{
    public interface IOrderRepo
    {
        Task PostOrder(OrderWrapper order);
        Task<IEnumerable<Order>> GetOrders(string email);
        Task<OrderWrapper> GetOrderById(Guid userId, Guid orderId);
    }
}
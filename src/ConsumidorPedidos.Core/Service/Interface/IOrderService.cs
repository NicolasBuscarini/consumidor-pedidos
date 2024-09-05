using ConsumidorPedidos.Model;

namespace ConsumidorPedidos.Core.Service.Interface
{
    public interface IOrderService
    {
        Task<Order> CreateOrder(Order order);
        Task<bool> QueueOrder(Order order);
        Task<bool> DeleteOrder(int id);
        Task<List<Order>> GetAllOrder();
        Task<Order> GetOrderById(int id);
        Task<Order?> UpdateOrder(Order order);
    }
}
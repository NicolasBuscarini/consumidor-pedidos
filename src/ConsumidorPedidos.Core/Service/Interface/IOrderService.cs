using ConsumidorPedidos.Model;
using ConsumidorPedidos.Model.Response;

namespace ConsumidorPedidos.Core.Service.Interface
{
    public interface IOrderService
    {
        Task<Order> CreateOrder(Order order);
        Task<bool> QueueOrder(Order order);
        Task<bool> DeleteOrder(int id);
        Task<(List<Order> Orders, MetaData Meta)> GetAllOrder(int pageNumber, int pageSize);
        Task<(List<Order> Orders, MetaData Meta)> GetOrdersByClientCode(int clientCode, int pageNumber, int pageSize);
        Task<Order> GetOrderById(int id);
        Task<Order?> UpdateOrder(Order order);
    }
}
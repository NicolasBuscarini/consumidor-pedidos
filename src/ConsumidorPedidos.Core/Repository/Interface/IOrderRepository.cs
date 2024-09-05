using ConsumidorPedidos.Data.MySql.Repository.Interface;
using ConsumidorPedidos.Model;

namespace ConsumidorPedidos.Core.Repository.Interface
{
    public interface IOrderRepository : IGenericRepository<Order, int>
    {
        Task<bool> Queue(Order order);
    }
}
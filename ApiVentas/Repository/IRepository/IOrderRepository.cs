using ApiVentas.Models;

namespace ApiVentas.Repository.IRepository
{
    public interface IOrderRepository
    {
        ICollection<Order> GetOrders();

        ICollection<Order> GetOrdersByCustomer(long id);

        Order GetOrder(long id);

        //Order GetOrderByVendor(long vendorId);

        bool ExistOrder(long id);

        bool ExistOrder(String name);

        bool CreateOrder(Order order);

        bool UpdateOrder(Order order);

        bool DeleteOrder(Order order);

        bool Save();
    }
}

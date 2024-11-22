using ApiVentas.Data;
using ApiVentas.Models;
using ApiVentas.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ApiVentas.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _db;

        // Constructor
        public OrderRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool CreateOrder(Order order)
        {
            order.UpdatedAt = DateTime.Now;
            order.CreatedAt = DateTime.Now;
            _db.Orders.Add(order);
            return Save();
        }

        public bool DeleteOrder(Order order)
        {
            order.UpdatedAt = DateTime.Now;
            order.DeletedAt = DateTime.Now;
            _db.Orders.Update(order);
            return Save();
        }

        public bool ExistOrder(long id)
        {
            return _db.Orders.Any(x => x.Id == id && x.DeletedAt == null);
        }

        public bool ExistOrder(string name)
        {
             bool valid = _db.Orders.Any(x => x.Name.ToLower().Trim() == name.ToLower().Trim() && x.DeletedAt == null);
            return valid;
        }

        public Order GetOrder(long id)
        {
            return _db.Orders.Include(x => x.Customer).FirstOrDefault(x => x.Id == id && x.DeletedAt == null);
        }

        public ICollection<Order> GetOrders()
        {
            return _db.Orders.Include(x => x.Customer).Where(x => x.DeletedAt == null).OrderBy(x => x.Name).ToList();
        }

        public bool Save()
        {
            // Se guarda solo cuando exista algun cambio
            return _db.SaveChanges() >= 0? true : false;
        }

        public ICollection<Order> GetOrdersByCustomer(long id)
        {
            return _db.Orders.Include(x => x.Customer).Where(x => x.CustomerId == id && x.DeletedAt == null).ToList();
        }

        public bool UpdateOrder(Order order)
        {
            order.UpdatedAt = DateTime.Now;

            // valida el put 
            var orderExist = _db.Orders.Find(order.Id);

            if (orderExist != null) {
                //  actualizar los valores del cliente en la base de datos sin reemplazar completamente el objeto
                // modificar los campos que han cambiado y evita crear duplicados
                _db.Entry(orderExist).CurrentValues.SetValues(order);
            }
            else {

                // Esto actualiza si lo encontrara sino hace un insert 
                _db.Orders.Update(order);
            }
            
            return Save();
        }
    }
}

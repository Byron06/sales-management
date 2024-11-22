using ApiVentas.Data;
using ApiVentas.Models;
using ApiVentas.Repository.IRepository;

namespace ApiVentas.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _db;

        // Constructor
        public CustomerRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool CreateCustomer(Customer customer)
        {
            customer.UpdatedAt = DateTime.Now;
            customer.CreatedAt = DateTime.Now;
            _db.Customers.Add(customer);
            return Save();
        }

        public bool DeleteCustomer(Customer customer)
        {
            customer.UpdatedAt = DateTime.Now;
            customer.DeletedAt = DateTime.Now;
            _db.Customers.Update(customer);
            return Save();
        }

        public bool ExistCustomer(long id)
        {
            return _db.Customers.Any(x => x.Id == id && x.DeletedAt == null);
        }

        public bool ExistCustomer(string name)
        {
             bool valid = _db.Customers.Any(x => x.Name.ToLower().Trim() == name.ToLower().Trim() && x.DeletedAt == null);
            return valid;
        }

        public Customer GetCustomer(long id)
        {
            return _db.Customers.FirstOrDefault(x => x.Id == id && x.DeletedAt == null);
        }

        public ICollection<Customer> GetCustomers()
        {
            return _db.Customers.Where(x => x.DeletedAt == null).OrderBy(x => x.Name).ToList();
        }

        public ICollection<Customer> GetSearchCustomers(string text)
        {
            IQueryable<Customer> query = _db.Customers;

            if (!string.IsNullOrEmpty(text)) { 
                query = query.Where(x => x.Name.Contains(text) || x.Phone.Contains(text));
            }

            return query.ToList();
        }

        public bool Save()
        {
            // Se guarda solo cuando exista algun cambio
            return _db.SaveChanges() >= 0? true : false;
        }

        public bool UpdateCustomer(Customer customer)
        {
            customer.UpdatedAt = DateTime.Now;

            // valida el put 
            var customerExist = _db.Customers.Find(customer.Id);

            if (customerExist != null) {
                //  actualizar los valores del cliente en la base de datos sin reemplazar completamente el objeto
                // modificar los campos que han cambiado y evita crear duplicados
                _db.Entry(customerExist).CurrentValues.SetValues(customer);
            }
            else {

                // Esto actualiza si lo encontrara sino hace un insert 
                _db.Customers.Update(customer);
            }
            
            return Save();
        }
    }
}

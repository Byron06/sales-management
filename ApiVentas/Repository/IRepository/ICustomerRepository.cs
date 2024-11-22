using ApiVentas.Models;

namespace ApiVentas.Repository.IRepository
{
    public interface ICustomerRepository
    {
        ICollection<Customer> GetCustomers();

        ICollection<Customer> GetSearchCustomers(string text);

        Customer GetCustomer(long id);

        //Customer GetCustomerByVendor(long vendorId);

        bool ExistCustomer(long id);

        bool ExistCustomer(String name);

        bool CreateCustomer(Customer customer);

        bool UpdateCustomer(Customer customer);

        bool DeleteCustomer(Customer customer);

        bool Save();
    }
}

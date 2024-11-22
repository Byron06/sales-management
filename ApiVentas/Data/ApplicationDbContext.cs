using ApiVentas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiVentas.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options) : base(options)
        {
            
        }

        // Todos los modelos deben estar aqui para que se representen en la BD
        public DbSet <Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<Vendor> Vendors { get; set; }
    }
}

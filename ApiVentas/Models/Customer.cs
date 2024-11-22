using System.ComponentModel.DataAnnotations;

namespace ApiVentas.Models
{
    public class Customer
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Phone { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}

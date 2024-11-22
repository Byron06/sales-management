using System.ComponentModel.DataAnnotations;

namespace ApiVentas.Models.Dtos
{
    public class OrderDto
    {
        public long id { get; set; }

        [MaxLength(100, ErrorMessage = "El nombre no debe ser mayor a 100")]
        public string name { get; set; }

        public long customerId { get; set; }

        public DateTime createdAt { get; set; }

        public DateTime? updatedAt { get; set; }

        public DateTime? deletedAt { get; set; }
    }

    public class OrderCreateDto
    {
        
        [MaxLength(100, ErrorMessage = "El nombre no debe ser mayor a 100")]
        public string name { get; set; }

        public long customerId { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace ApiVentas.Models.Dtos
{
    public class CustomerDto
    {
        public long id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(100,ErrorMessage = "El nombre no debe ser mayor a 100")]
        public string name { get; set; }

        public string phone { get; set; }

        public DateTime createdAt { get; set; }

        public DateTime? updatedAt { get; set; }

        public DateTime? deletedAt { get; set; }
    }

    public class CustomerCreateDto
    {

        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(100, ErrorMessage = "El nombre no debe ser mayor a 100")]
        public string name { get; set; }

        public string phone { get; set; }
    }
}

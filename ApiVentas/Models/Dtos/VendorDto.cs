using System.ComponentModel.DataAnnotations;

namespace ApiVentas.Models.Dtos
{
    public class VendorDto
    {
        public long id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(100, ErrorMessage = "El nombre no debe ser mayor a 100")]
        public string name { get; set; }

        public string email { get; set; }
        
        public string password { get; set; }

        public DateTime createdAt { get; set; }

        public DateTime? updatedAt { get; set; }

        public DateTime? deletedAt { get; set; }
    }

    public class VendorRegisterDto
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(100, ErrorMessage = "El nombre no debe ser mayor a 100")]
        public string name { get; set; }

        [Required(ErrorMessage = "El email es requerido")]
        public string email { get; set; }

        [Required(ErrorMessage = "El passsword es requerido")]
        public string password { get; set; }
    }

    public class VendorLoginDto
    {

        [Required(ErrorMessage = "El email es requerido")]
        public string email { get; set; }

        [Required(ErrorMessage = "El passsword es requerido")]
        public string password { get; set; }
    }

    public class VendorLoginResultDto
    {

        public UserDataDto user { get; set; }        
        public string token { get; set; }
    }


    public class UserDataDto
    {

        public long id { get; set; }
        public string name { get; set; }

        public string email { get; set; }
    }
}

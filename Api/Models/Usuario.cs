using System.ComponentModel.DataAnnotations;

namespace Api.Models
{
    public class Usuario
    {
        [Key]
        public string Cedula { get; set; }
        public string Username { get; set; }
        public byte[] PassHash { get; set; }
        public byte[] PassSalt { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Auth.API.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        // Relación con Role
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}

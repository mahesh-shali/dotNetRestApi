using System.ComponentModel.DataAnnotations;

namespace RestApi.Models
{
    public class User
    {
        public long UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }

        [MaxLength(255)] // Adjust the size to suit hashed passwords
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public long RoleId { get; set; }

        public Role? Role { get; set; } // Navigation property
    }
}

using System.ComponentModel.DataAnnotations;

namespace RestApi.Models
{
    public class User
    {
        public long UserId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Phone is required.")]
        public long Phone { get; set; }

        // [Required(ErrorMessage = "RoleId is required.")]
        public long? RoleId { get; set; }

        public Role? Role { get; set; } // Navigation property

        public DateTime createdAt { get; set; }

        public DateTime? modifiedAt { get; set; }
    }
}

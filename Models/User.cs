using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        public string Phone { get; set; }

        // [Required(ErrorMessage = "RoleId is required.")]
        public long? RoleId { get; set; }

        public Role? Role { get; set; } // Navigation property

        public DateTime createdAt { get; set; }

        public DateTime? modifiedAt { get; set; }

        public string? IpAddress { get; set; }

        public string? Browser { get; set; }

        public string? BrowserVersion { get; set; }

        public string? OS { get; set; }

        [JsonPropertyName("selectedCode")]
        public string? PhonePrefix { get; set; }

        public bool? IsEmailVerified { get; set; }

        public bool? IsPhoneNumberVerified { get; set; }

        public bool? IsLoggedInByGoogleId { get; set; }

        public string? Street { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? PostalCode { get; set; }

        public string? Country { get; set; }

        public bool? IsLoggedInByFaceBookId { get; set; }
    }
}

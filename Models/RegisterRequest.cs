using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class RegisterRequest
{
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Phone number is required.")]
    public string Phone { get; set; }

    public long? RoleId { get; set; }

    [JsonPropertyName("selectedCode")]
    public string? PhonePrefix { get; set; }

    [JsonPropertyName("ip")]
    public string? IpAddress { get; set; }

    [JsonPropertyName("browserInfo")]
    public BrowserInfo? BrowserInfo { get; set; }

    [JsonPropertyName("osName")]
    public string? OS { get; set; }
}

public class BrowserInfo
{
    [JsonPropertyName("browserName")]
    public string? Browser { get; set; }

    [JsonPropertyName("browserVersion")]
    public string? BrowserVersion { get; set; }
}

using System.ComponentModel.DataAnnotations;

public class LoginRequest
{
    [Required(ErrorMessage = "Email or Username is required.")]
    public string EmailOrUsername { get; set; }  // Can accept either email or username

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; }
}

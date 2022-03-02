namespace BlazorGameDotNet6.Shared;

public class UserLogin
{
    [Required(ErrorMessage = "Please enter an email!"), EmailAddress(ErrorMessage = "Please enter a valid email address!")]
    public string Email { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
}

namespace BlazorGameDotNet6.Shared;

public class UserRegister
{
    [Required, EmailAddress]
    public string? Email { get; set; }
    [Required, StringLength(16, ErrorMessage = "Your username is too long (16 characters max)")]
    public string? Username { get; set; }
    public string? Bio { get; set; }
    [Required, StringLength(100, MinimumLength = 6)]
    public string? Password { get; set; }
    [Compare("Password", ErrorMessage = "Passwords do not match!")]
    public string? ConfirmPassword { get; set; }
    public int StartUnitId { get; set; } = (int)UnitTypeEnum.Knight; // Knight by default
    public DateTime DateOfBirth { get; set; } = DateTime.Now;
    [Range(typeof(bool), "true", "true", ErrorMessage = "Only confirmed users can play!")] // Form should be only valid, if this is true! 
    public bool IsConfirmed { get; set; } = true;
}

using System.ComponentModel.DataAnnotations;

public class LoginModel
{
    [Required]
    [Display(Name = "Username or Email")]
    public string UsernameOrEmail { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [Display(Name = "Remember Me")]
    public bool RememberMe { get; set; }

    public string ReturnUrl { get; set; } 
}

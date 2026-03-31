using System.ComponentModel.DataAnnotations;

namespace TravelManagementApp.ViewModels;

public class LoginViewModel
{
    [Required]
    [StringLength(30)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [StringLength(100)]
    public string Password { get; set; } = string.Empty;
}
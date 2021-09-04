using System.ComponentModel.DataAnnotations;

namespace CognitoUserManagement.Application.DTOs
{
    public class UserSignUpModel
    {
        [Required(ErrorMessage = "An  full name is required")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "An email address is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "A confirmed password is required")]
        [Compare(nameof(Password), ErrorMessage = "Passwords must match")]
        [DataType(DataType.Password)]
        public string ConfirmedPassword { get; set; } = string.Empty;
    }
}
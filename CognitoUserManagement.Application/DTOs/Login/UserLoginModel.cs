using System.ComponentModel.DataAnnotations;

namespace CognitoUserManagement.Application.DTOs
{
    public class UserLoginModel
    {
        [Required(ErrorMessage = "An email address is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
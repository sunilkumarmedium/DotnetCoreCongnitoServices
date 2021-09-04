using System.ComponentModel.DataAnnotations;

namespace CognitoUserManagement.Application.DTOs
{
    public class UserConfirmSignUpModel
    {
        [Required(ErrorMessage = "An email address is required")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "An confirmation code is required")]
        public string ConfirmationCode { get; set; }
        public string UserId { get; set; }
        
        
    }
}
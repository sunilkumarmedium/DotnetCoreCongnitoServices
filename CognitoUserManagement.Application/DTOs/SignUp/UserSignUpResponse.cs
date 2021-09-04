namespace CognitoUserManagement.Application.DTOs
{
    public class UserSignUpResponse
    {
        public string UserId { get; set; }
        public string EmailAddress { get; set; }
        public bool UserConfirmed { get; set; }
        public string Message { get; set; }
        
        
    }
}
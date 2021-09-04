using System.Threading.Tasks;
using CognitoUserManagement.Application.DTOs;

namespace CognitoUserManagement.Application.Repositories
{
    public interface ICognitoAuthRepository
    {
        /* Signup Flow Starts */
        Task<UserSignUpResponse> ConfirmUserSignUpAsync(UserConfirmSignUpModel model);
        Task<UserSignUpResponse> CreateUserAsync(UserSignUpModel model);
        /* Signup Flow Ends */
        
        /* Change Password Flow */
        Task<BaseResponseModel> TryChangePasswordAsync(ChangePwdModel model);
        
        /* Forgot Password Flow Starts */
        Task<InitForgotPwdResponse> TryInitForgotPasswordAsync(InitForgotPwdModel model);
        Task<ResetPasswordResponse> TryResetPasswordWithConfirmationCodeAsync(ResetPasswordModel model);
        /* Forgot Password Flow Ends */
        
        /* Login Flow Starts */
        Task<AuthResponseModel> TryLoginAsync(UserLoginModel model);
        /* Login Flow Ends */
        
        Task<UserSignOutResponse> TryLogOutAsync(UserSignOutModel model);
        
        /* Update Profile Flow Starts */
        Task<UserProfileResponse> GetUserAsync(string userId);
        Task<UpdateProfileResponse> UpdateUserAttributesAsync(UpdateProfileModel model);
        /* Update Profile Flow Ends */
    }
}
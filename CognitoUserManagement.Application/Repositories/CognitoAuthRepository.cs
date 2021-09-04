using System.Threading.Tasks;
using CognitoUserManagement.Application.DTOs;
using CognitoUserManagement.Domain.Entities;

namespace CognitoUserManagement.Application.Repositories
{
    public class CognitoAuthRepository : ICognitoAuthRepository
    {
        private readonly CognitoConfig _cognitoConfig;
        public CognitoAuthRepository(CognitoConfig cognitoConfig)
        {
            this._cognitoConfig = cognitoConfig;
        }

        public Task<UserSignUpResponse> ConfirmUserSignUpAsync(UserConfirmSignUpModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task<UserSignUpResponse> CreateUserAsync(UserSignUpModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task<UserProfileResponse> GetUserAsync(string userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<BaseResponseModel> TryChangePasswordAsync(ChangePwdModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task<InitForgotPwdResponse> TryInitForgotPasswordAsync(InitForgotPwdModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task<AuthResponseModel> TryLoginAsync(UserLoginModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task<UserSignOutResponse> TryLogOutAsync(UserSignOutModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task<ResetPasswordResponse> TryResetPasswordWithConfirmationCodeAsync(ResetPasswordModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task<UpdateProfileResponse> UpdateUserAttributesAsync(UpdateProfileModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}
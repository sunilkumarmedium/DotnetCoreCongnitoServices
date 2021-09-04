using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using CognitoUserManagement.Application.DTOs;
using CognitoUserManagement.Domain.Entities;

namespace CognitoUserManagement.Infrastructure.Shared.Services
{
    public class CognitoAuthenticationService : ICognitoAuthenticationService
    {
        private readonly IAmazonCognitoIdentityProvider _identityProvider;
        private readonly CognitoConfig _cognitoConfig;
        public CognitoAuthenticationService(IAmazonCognitoIdentityProvider identityProvider, CognitoConfig cognitoConfig = null)
        {
            _identityProvider = identityProvider ?? throw new ArgumentNullException(nameof(identityProvider));
            _cognitoConfig = cognitoConfig;
        }

        public async Task<UserSignUpResponse> ConfirmUserSignUpAsync(UserConfirmSignUpModel model)
        {
            ConfirmSignUpRequest request = new ConfirmSignUpRequest
            {
                ClientId = _cognitoConfig.UserPoolClientId,
                ConfirmationCode = model.ConfirmationCode,
                Username = model.EmailAddress
            };

             var response = await _identityProvider.ConfirmSignUpAsync(request).ConfigureAwait(false);
            return new UserSignUpResponse
            {
                EmailAddress = model.EmailAddress,
                UserId = model.UserId,
                Message = "User Confirmed",
            };
        }

        public async Task<UserSignUpResponse> CreateUserAsync(UserSignUpModel signup)
        {
            var signUpRequest = new SignUpRequest
            {
                ClientId = _cognitoConfig.UserPoolClientId,
                Password = signup.Password,
                SecretHash = ComputeHash(signup.Email),
                UserAttributes = new List<AttributeType>
                {
                    new AttributeType { Name = "email", Value = signup.Email},
                    new AttributeType { Name = "given_name", Value = signup.FullName}
                },
                Username = signup.Email
            };

            SignUpResponse response = await _identityProvider.SignUpAsync(signUpRequest).ConfigureAwait(true);
            var signUpResponse = new UserSignUpResponse
            {
                UserId = response.UserSub,
                EmailAddress = signup.Email,
                UserConfirmed = response.UserConfirmed,
                Message = $"Confirmation Code sent to {response.CodeDeliveryDetails.Destination} via {response.CodeDeliveryDetails.DeliveryMedium.Value}",
                //IsSuccess = true
            };

        return signUpResponse;
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

        public async Task<AuthResponseModel> TryLoginAsync(UserLoginModel model)
        {
             var request = new AdminInitiateAuthRequest
            {
                ClientId = _cognitoConfig.UserPoolClientId,
                UserPoolId = _cognitoConfig.UserPoolId,
                AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH,
            };

            // For ADMIN_NO_SRP_AUTH: USERNAME (required), SECRET_HASH (if app client is configured
            // with client secret), PASSWORD (required)
            request.AuthParameters.Add("USERNAME", model.Email);
            request.AuthParameters.Add("PASSWORD", model.Password);
            request.AuthParameters.Add("SECRET_HASH", ComputeHash(model.Email));

            var response = await _identityProvider.AdminInitiateAuthAsync(request).ConfigureAwait(true);
            var result = response.AuthenticationResult;
            return new AuthResponseModel
            {
                EmailAddress = model.Email,
                //authResponseModel.UserId = user.Username;
                Tokens = new Application.DTOs.TokenModel
                {
                    IdToken = result.IdToken,
                    AccessToken = result.AccessToken,
                    ExpiresIn = result.ExpiresIn,
                    RefreshToken = result.RefreshToken
                }
            };
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

        public string ComputeHash(string username)
        {
            var dataString = username + _cognitoConfig.UserPoolClientId;

            var data = Encoding.UTF8.GetBytes(dataString);
            var key = Encoding.UTF8.GetBytes(_cognitoConfig.UserPoolClientSecret);

            return Convert.ToBase64String(HmacSHA256(data, key));
        }
        private static byte[] HmacSHA256(byte[] data, byte[] key)
        {
            var shaAlgorithm = new HMACSHA256(key);
            return shaAlgorithm.ComputeHash(data);
        }
    }
}
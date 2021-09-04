using System;
using System.Text;
using Amazon.CognitoIdentityProvider;
using CognitoUserManagement.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace CognitoUserManagementApi.Extensions
{
    public static class CognitoAuthenticationConfig
    {
        public static IServiceCollection AddCognitoAuthentication(this IServiceCollection services, IConfiguration configuration, CognitoConfig cognitoConfig)
        {
            //services.Configure<CognitoConfig>(configuration.GetSection("CognitoConfig"));

            // Setup AWS configuration and AWS Cognito Identity
            var defaultOptions = configuration.GetAWSOptions();
           // var cognitoOptions = configuration.GetAWSCognitoClientOptions();
            services.AddDefaultAWSOptions(defaultOptions);
            //services.AddSingleton(cognitoOptions);
            //services.AddSingleton(new CognitoClientSecret());
            services.AddAWSService<IAmazonCognitoIdentityProvider>();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                                    .AddJwtBearer(x =>
                                    {
                                        var authority = $"https://cognito-idp.{cognitoConfig.Region}.amazonaws.com/{cognitoConfig.UserPoolId}";
                                        x.Audience = cognitoConfig.UserPoolClientId; //configuration["JWTSettings:UserPoolClientId"];
                                        x.Authority = authority;
                                        x.TokenValidationParameters = new TokenValidationParameters()
                                        {
                                            ValidateIssuerSigningKey = true,
                                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cognitoConfig.UserPoolClientSecret)), //new CognitoSigningKey(configuration["JWTSettings:UserPoolClientSecret"]).ComputeKey(),
                                            ValidateIssuer = true,
                                            ValidateAudience = false,
                                            ValidateLifetime = true,
                                            ValidIssuer = authority,
                                            ValidAudience = cognitoConfig.UserPoolClientId,
                                            // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                                            ClockSkew = TimeSpan.Zero,
                                        };
                                        x.RequireHttpsMetadata = false;
                                        x.SaveToken = true;
                                        x.Events = new JwtBearerEvents()
                                        {
                                            OnAuthenticationFailed = c =>
                                            {
                                                c.NoResult();
                                                c.Response.StatusCode = 500;
                                                c.Response.ContentType = "text/plain";
                                                return c.Response.WriteAsync(c.Exception.ToString());
                                            },
                                            OnChallenge = context =>
                                            {
                                                if (!context.Response.HasStarted)
                                                {
                                                    context.Response.StatusCode = 401;
                                                    context.Response.ContentType = "application/json";
                                                    context.HandleResponse();
                                                    var result = "You are not Authorized";
                                                    return context.Response.WriteAsync(result);
                                                }
                                                else
                                                {
                                                    var result = "Token Expired";
                                                    return context.Response.WriteAsync(result);
                                                }
                                            },
                                            OnForbidden = context =>
                                            {
                                                context.Response.StatusCode = 403;
                                                context.Response.ContentType = "application/json";
                                                var result = "You are not authorized to access this resource";
                                                return context.Response.WriteAsync(result);
                                            },
                                        };
                                    });
            return services;
        }
    }
}
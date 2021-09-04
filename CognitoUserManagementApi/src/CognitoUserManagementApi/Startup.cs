using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CognitoUserManagement.Domain.Entities;
using CognitoUserManagementApi.Extensions;

namespace CognitoUserManagementApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            var cognitoConfig = new CognitoConfig();
            Configuration.Bind(nameof(CognitoConfig), cognitoConfig);
            services.AddSingleton(cognitoConfig);

            services.AddCognitoAuthentication(Configuration, cognitoConfig);
            services.AddControllers();
            services.AddSwaggerConfiguration();
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder.AllowAnyOrigin()
                                       .AllowAnyHeader()
                                       .AllowAnyMethod());
            app.UseSwaggerSetup();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Welcome to running ASP.NET Core on AWS Lambda");
                });
            });
        }
    }
}

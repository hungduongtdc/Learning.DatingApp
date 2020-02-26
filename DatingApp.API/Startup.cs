using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Core.HashingHelper;
using DatingApp.API.Core.Users;
using DatingApp.API.Data;
using DatingApp.API.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace DatingApp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DataContext>(x => x.UseSqlServer(connectionString));
            services.AddControllers(x => x.Filters.Add(typeof(ResponseFilter)));
            AddDependencies(services);
            services.AddCors();
            string secretKey = Configuration.GetSection("AppSettings:LoginKey").Value;
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                     ,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        private void AddDependencies(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<DataContext, DataContext>();
            services.AddScoped<IHashingHelper, HashingHelper>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // if (env.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage();
            // }
            // else
            HandleInternalServerError(app);
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors(x =>
            {

                x.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }

        private static void HandleInternalServerError(IApplicationBuilder app)
        {
            app.UseExceptionHandler(options =>
            {

                options.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    var error = context.Features.Get<IExceptionHandlerFeature>();
                    if (error != null)
                    {
                        string responseText = JsonConvert.SerializeObject(new BaseResponseModel()
                        {
                            Message = error.Error.Message
                        });
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(responseText);
                    }
                });
            });
        }
    }
}
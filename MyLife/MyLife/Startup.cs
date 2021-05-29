using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MyLife.Data;
using MyLife.Models;
using MyLife.Models.TargetModels;
using MyLife.Profiles;
using MyLife.Repositories;
using MyLife.Services.AccountServices;
using MyLife.Services.PasswordHashers;
using MyLife.Services.TargetServices;
using MyLife.Services.TokenGenerators;
using MyLife.Services.TokenValidators;
using MyLife.ViewModels;
using System;
using System.Text;

namespace MyLife
{
    public class Startup
    {
        public IConfiguration Configuration;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<RouteOptions>(options =>
            {
                options.AppendTrailingSlash = true;
                options.LowercaseUrls = true;
            });

            var accessTokenConfig = Configuration.GetSection("Authentication");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(accessTokenConfig["AccessTokenSecret"])),
                    ValidIssuer = accessTokenConfig["Issuer"],
                    ValidAudience = accessTokenConfig["Audience"],
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddScoped<ApplicationContext>();
            services.AddSingleton<IRepository<User>, UserInMemoryRepository>();
            services.AddSingleton<IRepository<Target>, TargetInMemoryRepository>();

            services.AddTransient<IAccountService, AccountService>();
            services.AddScoped<TokenGenerator>();
            services.AddTransient<AccessTokenGenerator>();
            services.AddTransient<RefreshTokenGenerator>();

            services.AddTransient<IPasswordHasher, BcryptPasswordHasher>();
            services.AddTransient<RefreshTokenValidator>();

            services.AddTransient<ITargetService, TargetService>();

            //services.AddAutoMapper(cfg => cfg.CreateMap<User, ProfileViewModel>() ,typeof(Startup));

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddTransient<IMapper, Mapper>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

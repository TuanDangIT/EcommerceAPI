using Ecommerce.Modules.Users.Core.DAL.Repositories;
using Ecommerce.Modules.Users.Core.DAL;
using Ecommerce.Modules.Users.Core.Entities;
using Ecommerce.Modules.Users.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Ecommerce.Shared.Infrastructure.Postgres;
using Microsoft.Extensions.Configuration;
using Sieve.Models;
using Sieve.Services;
using Ecommerce.Modules.Users.Core.Sieve;

[assembly: InternalsVisibleTo("Ecommerce.Modules.Users.Api")]
namespace Ecommerce.Modules.Users.Core
{
    internal static class Extensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddPostgres<UsersDbContext>();
            //services.AddIdentityCore<UsersDbContext>(options =>
            //{
            //    //email
            //    options.SignIn.RequireConfirmedAccount = false;
            //    options.SignIn.RequireConfirmedEmail = true;
            //    //options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
            //    //lockout
            //    options.Lockout.MaxFailedAccessAttempts = 5;
            //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            //})
            //    .AddRoles<Role>()
            //    .AddEntityFrameworkStores<UsersDbContext>()
            //    .AddDefaultTokenProviders();
            services.Configure<SieveOptions>(configuration.GetSection("Sieve"));
            services.AddScoped<ISieveProcessor, UsersModuleSieveProcessor>();
            return services;
        }
    }
}

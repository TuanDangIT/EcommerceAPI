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

[assembly: InternalsVisibleTo("Ecommerce.Modules.Users.Api")]
namespace Ecommerce.Modules.Users.Core
{
    internal static class Extensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddPostgres<UsersDbContext>();
            return services;
        }
    }
}

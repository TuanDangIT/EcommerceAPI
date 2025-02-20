using Ecommerce.Modules.Users.Core.DAL.Repositories;
using Ecommerce.Modules.Users.Core.Entities;
using Ecommerce.Shared.Infrastructure.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Core.BackgroundServices
{
    internal class AddAdmin : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AdminOptions _adminOptions;

        public AddAdmin(IServiceProvider serviceProvider, IPasswordHasher<User> passwordHasher, AdminOptions adminOptions)
        {
            _serviceProvider = serviceProvider;
            _passwordHasher = passwordHasher;
            _adminOptions = adminOptions;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("--------------------");
            using var scope = _serviceProvider.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var employeeRepository = scope.ServiceProvider.GetRequiredService<IEmployeeRepository>();
            var admin = await employeeRepository.GetAsync(_adminOptions.Id, false, cancellationToken);
            if(admin is null)
            {
                var roleRepository = scope.ServiceProvider.GetRequiredService<IRoleRepository>();
                var role = await roleRepository.GetAsync(_adminOptions.Role, cancellationToken) ?? throw new ArgumentNullException();
                await employeeRepository.AddAsync(new Employee(_adminOptions.Id, _adminOptions.Role, _adminOptions.Role, _adminOptions.Email,
                    _passwordHasher.HashPassword(default!, _adminOptions.Password), _adminOptions.Role, role, _adminOptions.Role), cancellationToken);
                return;
            }
            if(admin.Email != _adminOptions.Email || 
                _passwordHasher.VerifyHashedPassword(admin, admin.Password, _adminOptions.Password) is PasswordVerificationResult.Failed)
            {
                admin.Email = _adminOptions.Email;
                admin.Password = _passwordHasher.HashPassword(default!, _adminOptions.Password);
                await userRepository.UpdateAsync(cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

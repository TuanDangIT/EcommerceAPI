using Ecommerce.Modules.Users.Core.DAL.Mappings;
using Ecommerce.Modules.Users.Core.DAL.Repositories;
using Ecommerce.Modules.Users.Core.DTO;
using Ecommerce.Modules.Users.Core.Entities;
using Ecommerce.Modules.Users.Core.Exceptions;
using Ecommerce.Shared.Infrastructure.Pagination;
using Microsoft.AspNetCore.Identity;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Core.Services
{
    internal class EmployeeService : IEmployeeService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IRoleRepository _roleRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly TimeProvider _timeProvider;

        public EmployeeService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, IRoleRepository roleRepository, IEmployeeRepository employeeRepository, TimeProvider timeProvider)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _roleRepository = roleRepository;
            _employeeRepository = employeeRepository;
            _timeProvider = timeProvider;
        }

        public async Task<PagedResult<EmployeeBrowseDto>> BrowseAsync(SieveModel model)
            => await _employeeRepository.GetAllAsync(model);

        public async Task<Guid> CreateAsync(EmployeeCreateDto dto)
        {
            var email = dto.Email.ToLowerInvariant();
            if (await _userRepository.GetByEmailAsync(email) is not null)
            {
                throw new EmailInUseException();
            }
            if (await _userRepository.GetByUsernameAsync(dto.Username) is not null)
            {
                throw new UsernameInUseException();
            }
            var password = _passwordHasher.HashPassword(default!, dto.Password);
            var role = await _roleRepository.GetAsync(dto.Role);
            var newGuid = Guid.NewGuid();
            var user = new Employee(newGuid, dto.FullName, email, password, dto.Username, role!, dto.JobPosition, _timeProvider.GetUtcNow().UtcDateTime);
            await _userRepository.AddAsync(user);
            return newGuid;
        }

        public async Task DeleteAsync(Guid employeeId)
            => await _userRepository.DeleteAsync(employeeId);

        public async Task<EmployeeDetailsDto> GetAsync(Guid employeeId)
        {
            var employee = await _employeeRepository.GetAsync(employeeId, true) ?? throw new EmployeeNotFoundException(employeeId);
            return employee.AsDetailsDto();
        }

        public async Task UpdateAsync(EmployeeUpdateDto dto)
        {
            var employee = await _employeeRepository.GetAsync(dto.EmployeeId, false) ?? throw new EmployeeNotFoundException(dto.EmployeeId);
            employee.FullName = dto.FullName;
            employee.JobPosition = dto.JobPosition;
            employee.Username = dto.Username;
            employee.Email = dto.Email;
            var role = await _roleRepository.GetAsync(dto.Role);
            employee.Role = role!;
            await _userRepository.UpdateAsync();
        }
        public async Task SetActiveAsync(Guid employeeId, bool isActive)
        {
            var employee = await _employeeRepository.GetAsync(employeeId, false) ?? throw new EmployeeNotFoundException(employeeId);
            employee.IsActive = isActive;
            employee.UpdatedAt = _timeProvider.GetUtcNow().UtcDateTime;
            await _userRepository.UpdateAsync();
        }
    }
}

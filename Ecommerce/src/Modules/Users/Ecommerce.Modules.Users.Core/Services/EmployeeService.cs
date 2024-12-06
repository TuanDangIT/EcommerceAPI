using Ecommerce.Modules.Users.Core.DAL.Mappings;
using Ecommerce.Modules.Users.Core.DAL.Repositories;
using Ecommerce.Modules.Users.Core.DTO;
using Ecommerce.Modules.Users.Core.Entities;
using Ecommerce.Modules.Users.Core.Exceptions;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<EmployeeService> _logger;
        private readonly IContextService _contextService;

        public EmployeeService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, IRoleRepository roleRepository, IEmployeeRepository employeeRepository,
            ILogger<EmployeeService> logger, IContextService contextService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _roleRepository = roleRepository;
            _employeeRepository = employeeRepository;
            _logger = logger;
            _contextService = contextService;
        }

        public async Task<PagedResult<EmployeeBrowseDto>> BrowseAsync(SieveModel model, CancellationToken cancellationToken = default)
            => await _employeeRepository.GetAllAsync(model, cancellationToken);

        public async Task<Guid> CreateAsync(EmployeeCreateDto dto, CancellationToken cancellationToken = default)
        {
            var email = dto.Email.ToLowerInvariant();
            if (await _userRepository.GetByEmailAsync(email, cancellationToken) is not null)
            {
                throw new EmailInUseException();
            }
            if (await _userRepository.GetByUsernameAsync(dto.Username, cancellationToken) is not null)
            {
                throw new UsernameInUseException();
            }
            var password = _passwordHasher.HashPassword(default!, dto.Password);
            var role = await _roleRepository.GetAsync(dto.Role, cancellationToken);
            var newGuid = Guid.NewGuid();
            var user = new Employee(newGuid, dto.FirstName, dto.LastName, email, password, dto.Username, role!, dto.JobPosition);
            await _userRepository.AddAsync(user, cancellationToken);
            _logger.LogInformation("Employee: {@employee} was created by {@user}.", dto, new { _contextService.Identity!.Username, _contextService.Identity!.Id });
            return newGuid;
        }

        public async Task DeleteAsync(Guid employeeId, CancellationToken cancellationToken = default)
        {
            await _userRepository.DeleteAsync(employeeId, cancellationToken);
            _logger.LogInformation("Employee: {employeeId} was deleted by {@user}.", employeeId, new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }

        public async Task<EmployeeDetailsDto> GetAsync(Guid employeeId, CancellationToken cancellationToken = default)
        {
            var employee = await _employeeRepository.GetAsync(employeeId, true, cancellationToken) ?? throw new EmployeeNotFoundException(employeeId);
            return employee.AsDetailsDto();
        }

        public async Task UpdateAsync(EmployeeUpdateDto dto, CancellationToken cancellationToken = default)
        {
            var employee = await _employeeRepository.GetAsync(dto.EmployeeId, false, cancellationToken) ?? throw new EmployeeNotFoundException(dto.EmployeeId);
            employee.FirstName = dto.FirstName;
            employee.LastName = dto.LastName;
            employee.JobPosition = dto.JobPosition;
            employee.Username = dto.Username;
            employee.Email = dto.Email;
            var role = await _roleRepository.GetAsync(dto.Role, cancellationToken);
            employee.Role = role!;
            await _userRepository.UpdateAsync(cancellationToken);
            _logger.LogInformation("Employee: {@employee} was updated with new details {@newDetails} by {@user}.", employee, dto, new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
        public async Task SetActiveAsync(Guid employeeId, bool isActive, CancellationToken cancellationToken = default)
        {
            var employee = await _employeeRepository.GetAsync(employeeId, false, cancellationToken) ?? throw new EmployeeNotFoundException(employeeId);
            employee.IsActive = isActive;
            await _userRepository.UpdateAsync(cancellationToken);
            _logger.LogInformation("Employee: {@employee} was set to {isActive} by {@user}.", employee, isActive, 
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
    }
}

using Ecommerce.Modules.Users.Core.DAL.Mappings;
using Ecommerce.Modules.Users.Core.DAL.Repositories;
using Ecommerce.Modules.Users.Core.DTO;
using Ecommerce.Modules.Users.Core.Exceptions;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Core.Services
{
    internal class CustomerService : ICustomerService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(IUserRepository userRepository, ICustomerRepository customerRepository)
        {
            _userRepository = userRepository;
            _customerRepository = customerRepository;
        }
        public async Task<PagedResult<CustomerBrowseDto>> BrowseAsync(SieveModel model)
            => await _customerRepository.GetAllAsync(model);

        public async Task DeleteAsync(Guid customerId)
            => await _userRepository.DeleteAsync(customerId);

        public async Task<CustomerDetailsDto> GetAsync(Guid customerId)
        {
            var customer = await _customerRepository.GetAsync(customerId, true) ?? throw new CustomerNotFoundException(customerId);
            return customer.AsDetailsDto();
        }

        public async Task UpdateAsync(CustomerUpdateDto dto)
        {
            var customer = await _customerRepository.GetAsync(dto.CustomerId, false) ?? throw new CustomerNotFoundException(dto.CustomerId);
            customer.FirstName = dto.FirstName;
            customer.LastName = dto.LastName;
            customer.Email = dto.Email;
            customer.Username = dto.Username;
            await _userRepository.UpdateAsync();
        }
        public async Task SetActiveAsync(Guid customerId, bool isActive)
        {
            var customer = await _customerRepository.GetAsync(customerId, false) ?? throw new CustomerNotFoundException(customerId);
            customer.IsActive = isActive;
            await _userRepository.UpdateAsync();
        }
    }
}

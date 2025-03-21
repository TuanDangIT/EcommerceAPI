using Ecommerce.Modules.Users.Core.DAL.Mappings;
using Ecommerce.Modules.Users.Core.DAL.Repositories;
using Ecommerce.Modules.Users.Core.DTO;
using Ecommerce.Modules.Users.Core.Events;
using Ecommerce.Modules.Users.Core.Exceptions;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.Messaging;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<CustomerService> _logger;
        private readonly IContextService _contextService;
        private readonly IMessageBroker _messageBroker;

        public CustomerService(IUserRepository userRepository, ICustomerRepository customerRepository,
            ILogger<CustomerService> logger, IContextService contextService, IMessageBroker messageBroker)
        {
            _userRepository = userRepository;
            _customerRepository = customerRepository;
            _logger = logger;
            _contextService = contextService;
            _messageBroker = messageBroker;
        }
        public async Task<PagedResult<CustomerBrowseDto>> BrowseAsync(SieveModel model, CancellationToken cancellationToken = default)
            => await _customerRepository.GetAllAsync(model, cancellationToken);

        public async Task DeleteAsync(Guid customerId, CancellationToken cancellationToken = default)
        {
            await _userRepository.DeleteAsync(customerId, cancellationToken);
            _logger.LogInformation("Customer: {customerId} was deleted by {@user}.", customerId, new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }

        public async Task<CustomerDetailsDto> GetAsync(Guid customerId, CancellationToken cancellationToken = default)
        {
            var customer = await _customerRepository.GetAsync(customerId, true, cancellationToken) ?? throw new CustomerNotFoundException(customerId);
            return customer.AsDetailsDto();
        }

        public async Task UpdateAsync(CustomerUpdateDto dto, CancellationToken cancellationToken = default)
        {
            var customer = await _customerRepository.GetAsync(dto.CustomerId, false) ?? throw new CustomerNotFoundException(dto.CustomerId);
            customer.FirstName = dto.FirstName;
            customer.LastName = dto.LastName;
            customer.Email = dto.Email;
            customer.Username = dto.Username;
            await _userRepository.UpdateAsync(cancellationToken);
            _logger.LogInformation("Customer: {cutomerId} was updated with new details {@newDetails} by {@user}.", customer.Id, dto, new { _contextService.Identity!.Username, _contextService.Identity!.Id });
        }
        public async Task SetActiveAsync(Guid customerId, bool isActive, CancellationToken cancellationToken = default)
        {
            var customer = await _customerRepository.GetAsync(customerId, false, cancellationToken) ?? throw new CustomerNotFoundException(customerId);
            customer.IsActive = isActive;
            await _userRepository.UpdateAsync(cancellationToken);
            _logger.LogInformation("Customer: {customerId} was set to {isActive} by {@user}.", customer.Id, isActive, 
                new { _contextService.Identity!.Username, _contextService.Identity!.Id });
            if(isActive)
            {
                await _messageBroker.PublishAsync(new CustomerActivated(customerId, customer.Email, customer.FirstName, customer.LastName));
            }
        }
    }
}

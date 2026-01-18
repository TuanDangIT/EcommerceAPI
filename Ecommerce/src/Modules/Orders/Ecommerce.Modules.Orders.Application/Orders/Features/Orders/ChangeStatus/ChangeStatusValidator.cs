using Ecommerce.Modules.Orders.Domain.Orders.Entities.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Order.ChangeStatus
{
    internal class ChangeStatusValidator : AbstractValidator<ChangeStatus>
    {
        private readonly string[] _availableStatuses = Enum.GetNames(typeof(OrderStatus)); 
        public ChangeStatusValidator()
        {
            RuleFor(c => c.OrderId)
                .NotEmpty()
                .NotNull();
            RuleFor(c => c.Status)
                .NotEmpty()
                .NotNull()
                .Custom((value, context) =>
                {
                    if (!_availableStatuses.Contains(value, StringComparer.OrdinalIgnoreCase))
                    {
                        context.AddFailure($"Provided status is invalid. Please use the following ones: {string.Join(", ", _availableStatuses)}.");
                    }
                });
        }
    }
}

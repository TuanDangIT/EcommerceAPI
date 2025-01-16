using Ecommerce.Modules.Orders.Domain.Returns.Entities;
using Ecommerce.Modules.Orders.Domain.Returns.Entities.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Returns.Features.Returns.SetReturnProductStatus
{
    internal class SetReturnProductStatusValidator : AbstractValidator<SetReturnProductStatus>
    {
        private readonly string[] _availableStatuses = Enum.GetNames(typeof(ReturnProductStatus));
        public SetReturnProductStatusValidator()
        {
            RuleFor(c => c.ReturnId)
                .NotEmpty()
                .NotNull();
            RuleFor(c => c.ProductId)
                .NotEmpty()
                .NotNull();
            RuleFor(c => c.Status)
                .NotEmpty()
                .NotNull()
                .Custom((value, context) =>
                {
                    if (!_availableStatuses.Contains(value, StringComparer.InvariantCultureIgnoreCase))
                    {
                        context.AddFailure($"Provided status is invalid. Please use the following ones: {string.Join(", ", _availableStatuses)}.");
                    }
                });
        }
    }
}

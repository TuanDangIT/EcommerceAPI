using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Order.EditCustomer
{
    internal class EditCustomerValidator : AbstractValidator<EditCustomer>
    {
        public EditCustomerValidator()
        {
            RuleFor(e => e.FirstName)
                .NotEmpty()
                .NotNull()
                .Length(2, 48);
            RuleFor(e => e.LastName)
                .NotEmpty()
                .NotNull()
                .Length(2, 48);
            RuleFor(e => e.Email)
                .NotEmpty()
                .NotNull()
                .Matches("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,10}$")
                .WithMessage((value) =>
                {
                    return $"Given email: {value.Email} is invalid.";
                });
            RuleFor(e => e.PhoneNumber)
                .NotEmpty()
                .NotNull()
                .Custom((value, context) =>
                {
                    ReadOnlySpan<char> valueSpan = value.Replace("+", string.Empty).AsSpan().TrimEnd();

                    bool digitFound = false;
                    foreach (char c in valueSpan)
                    {
                        if (char.IsDigit(c))
                        {
                            digitFound = true;
                            break;
                        }
                    }
                    if (!digitFound)
                    {
                        context.AddFailure($"Given phone number: {value} is invalid.");
                    }
                });
        }
    }
}

using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Shipment.CreateShipment
{
    internal class CreateShipmentValidator : AbstractValidator<CreateShipment>  
    {
        public CreateShipmentValidator()
        {
            RuleFor(c => c.OrderId)
                .NotEmpty()
                .NotNull();
            RuleFor(c => c.Parcels)
                .NotNull()
                .NotEmpty();
            RuleForEach(c => c.Parcels)
                .NotNull()
                .NotEmpty()
                .ChildRules(p =>
                {
                    p.RuleFor(p => p.Width)
                        .NotNull()
                        .NotEmpty()
                        .Custom((value, context) =>
                        {
                            if (!decimal.TryParse(value, out decimal result))
                            {
                                context.AddFailure($"Provided value: {value} is not type number/decimal.");
                            }
                            if (result <= 0)
                            {
                                context.AddFailure($"Provided value: {value} must be greater than 0.");
                            }
                        });
                    p.RuleFor(p => p.Weight)
                        .NotNull()
                        .NotEmpty()
                        .Custom((value, context) =>
                        {
                            if (!decimal.TryParse(value, out decimal result))
                            {
                                context.AddFailure($"Provided value: {value} is not type number/decimal.");
                            }
                            if (result <= 0)
                            {
                                context.AddFailure($"Provided value: {value} must be greater than 0.");
                            }
                        });
                    p.RuleFor(p => p.Height)
                        .NotNull()
                        .NotEmpty()
                        .Custom((value, context) =>
                        {
                            if (!decimal.TryParse(value, out decimal result))
                            {
                                context.AddFailure($"Provided value: {value} is not type number/decimal.");
                            }
                            if (result <= 0)
                            {
                                context.AddFailure($"Provided value: {value} must be greater than 0.");
                            }
                        });
                    p.RuleFor(p => p.Length)
                        .NotNull()
                        .NotEmpty()
                        .Custom((value, context) =>
                        {
                            if (!decimal.TryParse(value, out decimal result))
                            {
                                context.AddFailure($"Provided value: {value} is not type number/decimal.");
                            }
                            if (result <= 0)
                            {
                                context.AddFailure($"Provided value: {value} must be greater than 0.");
                            }
                        });
                });
        }
    }
}

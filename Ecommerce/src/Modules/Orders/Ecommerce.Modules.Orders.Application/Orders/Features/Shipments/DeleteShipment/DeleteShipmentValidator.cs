using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Application.Orders.Features.Shipment.DeleteShipment
{
    internal class DeleteShipmentValidator : AbstractValidator<DeleteShipment>
    {
        public DeleteShipmentValidator()
        {
            RuleFor(d => d.ShipmentId)
                .NotEmpty()
                .NotNull();
            RuleFor(d => d.OrderId)
                .NotEmpty()
                .NotNull();
        }
    }
}

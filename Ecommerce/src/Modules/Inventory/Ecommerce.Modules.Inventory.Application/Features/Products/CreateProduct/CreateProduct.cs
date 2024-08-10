using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ecommerce.Modules.Inventory.Application.Features.Products.CreateProduct
{
    public sealed record class CreateProduct() : Shared.Abstractions.MediatR.ICommand;
}

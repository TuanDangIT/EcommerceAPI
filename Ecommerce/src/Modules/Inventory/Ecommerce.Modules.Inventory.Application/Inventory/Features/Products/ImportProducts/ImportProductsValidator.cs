using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Application.Inventory.Features.Products.ImportProducts
{
    internal class ImportProductsValidator : AbstractValidator<ImportProducts>
    {
        private readonly Dictionary<string, char> _availableDelimiters = new()
        {
            { "semicolon", ';' },
            { "comma", ',' }
        };
        public ImportProductsValidator()
        {
            RuleFor(i => i.ImportFile)
                .NotEmpty()
                .NotNull();
            RuleFor(i => i.Delimiter)
                .NotEmpty()
                .NotNull()
                .Custom((value, context) =>
                {
                    if (!_availableDelimiters.ContainsValue(value))
                    {
                        context.AddFailure($"Provided delimiter is not supported. Please use the following ones: {string.Join(", ", _availableDelimiters)}.");
                    }
                });
        }
    }
}

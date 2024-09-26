using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Discounts.Core.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    internal class FutureDateTimeAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            return value is DateTime date && date > TimeProvider.System.GetUtcNow().UtcDateTime
                ? ValidationResult.Success
                : new ValidationResult("Date must be in the future");
        }
    }
}

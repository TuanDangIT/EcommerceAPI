using Microsoft.Extensions.DependencyInjection;
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
            if(value is null)
            {
                return ValidationResult.Success;
            }
            var timeProvider = validationContext.GetRequiredService<TimeProvider>();
            return value is DateTime date && date > timeProvider.GetUtcNow().UtcDateTime
                ? ValidationResult.Success
                : new ValidationResult("Date must be in the future");
        }
    }
}

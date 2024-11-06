using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Mails.Api.DataAnnotations
{
    internal class MailCursorAttribute : ValidationAttribute
    {
        private readonly string[] _availableFilters = ["Id", "From", "To", "Subject", "Body", "OrderId", "CreatedAt", "CustomerId",
            "Customer.Email", "Customer.FirstName", "Customer.LastName"];
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value is Dictionary<string, string> filters)
            {
                foreach(var filter in filters)
                {
                    if (_availableFilters.Contains(filter.Key))
                    {
                        return new ValidationResult($"Provided filter is not supported. Please use the following ones: {string.Join(", ", _availableFilters)}.");
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}

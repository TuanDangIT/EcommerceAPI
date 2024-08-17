using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Abstractions.Exceptions
{
    public class ValidationException : EcommerceException
    {
        public Error[] Errors { get; set; }
        public ValidationException(string message, Error[] errors) : base(message)
        {
            Errors = errors;
        }
    }
}

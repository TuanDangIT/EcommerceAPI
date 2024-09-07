using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Abstractions.Exceptions
{
    public class ValidationException : EcommerceException
    {
        public Dictionary<string, string[]> Errors { get; set; }
        public ValidationException(string title, Dictionary<string, string[]> errors) : base(title)
        {
            Errors = errors;
        }
    }
}

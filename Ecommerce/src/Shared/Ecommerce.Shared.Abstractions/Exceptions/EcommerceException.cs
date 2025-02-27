using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Abstractions.Exceptions
{
    public abstract class EcommerceException : Exception
    {
        public EcommerceException(string message) : base(message)
        {
            
        }
        public EcommerceException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}

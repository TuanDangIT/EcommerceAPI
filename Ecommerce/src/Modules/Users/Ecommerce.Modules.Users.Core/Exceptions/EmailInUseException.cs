using Ecommerce.Shared.Abstractions.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Users.Core.Exceptions
{
    internal class EmailInUseException : EcommerceException
    {
        public EmailInUseException(string message) : base(message)
        {
        }
        public EmailInUseException() : base("Email is already in use.")
        {
        }
    }
}

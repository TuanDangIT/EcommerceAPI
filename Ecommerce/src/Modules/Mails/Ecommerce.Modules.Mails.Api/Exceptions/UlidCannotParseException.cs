using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Mails.Api.Exceptions
{
    internal class UlidCannotParseException(string stringedUlid) : EcommerceException($"String: {stringedUlid} cannot be parsed to Ulid type.")
    {
    }
}

using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Mails.Api.Exceptions
{
    internal class MailNotFoundException(int mailId) : EcommerceException($"Mail: {mailId} was not found.")
    {
    }
}

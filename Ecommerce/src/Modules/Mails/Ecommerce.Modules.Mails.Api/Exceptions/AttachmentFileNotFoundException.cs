using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Mails.Api.Exceptions
{
    internal class AttachmentFileNotFoundException(int mailId, Ulid fileId) : EcommerceException($"File: {fileId} does not exist in mail with ID: {mailId}.")
    {
    }
}

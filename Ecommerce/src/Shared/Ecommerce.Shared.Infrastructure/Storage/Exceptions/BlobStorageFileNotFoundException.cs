using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Storage.Exceptions
{
    internal class BlobStorageFileNotFoundException(string fileName) : EcommerceException($"File name: {fileName} was not found.")
    {
    }
}

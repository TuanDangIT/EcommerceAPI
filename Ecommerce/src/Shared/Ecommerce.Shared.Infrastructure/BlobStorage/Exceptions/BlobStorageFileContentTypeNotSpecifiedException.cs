using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Ecommerce.Shared.Infrastructure.Tests.Unit")]
namespace Ecommerce.Shared.Infrastructure.BlobStorage.Exceptions
{
    internal class BlobStorageFileContentTypeNotSpecifiedException : EcommerceException
    {
        public BlobStorageFileContentTypeNotSpecifiedException() : base("File content type is not specified.")
        {
        }
    }
}

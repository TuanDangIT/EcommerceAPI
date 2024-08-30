using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Storage.Exceptions
{
    internal class BlobStorageFileNotDeletedException : EcommerceException
    {
        public string FileName { get; }
        public BlobStorageFileNotDeletedException(string fileName) : base("File was not deleted in a blob storage.")
        {
            FileName = fileName;
        }
    }
}

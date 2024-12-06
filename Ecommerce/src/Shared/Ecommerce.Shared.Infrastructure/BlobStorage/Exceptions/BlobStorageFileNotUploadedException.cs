using Ecommerce.Shared.Abstractions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Storage.Exceptions
{
    internal class BlobStorageFileNotUploadedException : EcommerceException
    {
        public string FileName { get; }
        public BlobStorageFileNotUploadedException(string fileName) : base("File was not uploaded in a blob storage.")
        {
            FileName = fileName;
        }
    }
}

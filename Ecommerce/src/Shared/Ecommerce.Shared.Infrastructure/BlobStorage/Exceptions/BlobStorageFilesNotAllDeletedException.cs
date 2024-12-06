using Ecommerce.Shared.Abstractions.Exceptions;
using MediatR.NotificationPublishers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Storage.Exceptions
{
    internal class BlobStorageFilesNotAllDeletedException : EcommerceException
    {
        IEnumerable<string> FileNames { get; }
        public BlobStorageFilesNotAllDeletedException(IEnumerable<string> fileNames) : base("One or more files were not deleted in a blob storage.")
        {
            FileNames = fileNames;
        }
    }
}

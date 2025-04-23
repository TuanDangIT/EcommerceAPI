using Ecommerce.Shared.Abstractions.BloblStorage;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Inventory.Tests.Integration
{
    internal class FakeBlobStorageService : IBlobStorageService
    {
        public Task DeleteAsync(string fileName, string containerName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteManyAsync(IEnumerable<string> fileNames, string containerName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<BlobStorageDto> DownloadAsync(string fileName, string containerName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<string> UploadAsync(IFormFile blob, string fileName, string containerName, CancellationToken cancellationToken = default)
        {
            return Task.FromResult("path");
        }
    }
}

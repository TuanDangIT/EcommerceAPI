using Azure.Storage.Blobs;
using Ecommerce.Shared.Abstractions.BloblStorage;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Infrastructure.Storage
{
    internal class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobStorageService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }
        public async Task DeleteAsync(string fileName, string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.DeleteAsync();
        }

        public async Task<string> UploadAsync(IFormFile blob, string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(blob.FileName);
            await using(Stream data = blob.OpenReadStream())
            {
                await blobClient.UploadAsync(data);
            }
            return blobClient.Uri.AbsoluteUri;
        }
    }
}

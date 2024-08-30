using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
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
            var a = blobClient.Uri;
            await blobClient.DeleteAsync();
        }
        public async Task DeleteManyAsync(IEnumerable<string> fileNames, string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobBatchClient blobBatchClient = containerClient.GetBlobBatchClient();
            var blobStorageUri = blobBatchClient.Uri;
            List<Uri> imageUris = new();
            foreach(var fileName in fileNames)
            {
                imageUris.Add(new Uri(blobStorageUri + "/" + fileName));
            }
            await blobBatchClient.DeleteBlobsAsync(imageUris);
        }
        public async Task<string> UploadAsync(IFormFile blob, string fileName, string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            await using(Stream data = blob.OpenReadStream())
            {
                await blobClient.UploadAsync(data, new BlobHttpHeaders()
                {
                    ContentType = blob.ContentType
                });
            }
            return blobClient.Uri.AbsolutePath;
        }
    }
}

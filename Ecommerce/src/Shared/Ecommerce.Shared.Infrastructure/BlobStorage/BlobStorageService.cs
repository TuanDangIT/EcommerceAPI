using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Ecommerce.Shared.Abstractions.BloblStorage;
using Ecommerce.Shared.Infrastructure.Storage.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Azure;
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
        private readonly IAzureClientFactory<BlobServiceClient> _factory;
        private const string _clientName = "Blob";

        public BlobStorageService(IAzureClientFactory<BlobServiceClient> factory)
        {
            _factory = factory;
            _blobServiceClient = _factory.CreateClient(_clientName);
        }
        public async Task DeleteAsync(string fileName, string containerName, CancellationToken cancellationToken = default)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            using var response = await blobClient.DeleteAsync(cancellationToken: cancellationToken);
            if (response.IsError)
            {
                throw new RequestFailedException($"Request failed. File: {fileName} was not deleted from blob storage.");
            }
        }
        public async Task DeleteManyAsync(IEnumerable<string> fileNames, string containerName, CancellationToken cancellationToken = default)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobBatchClient = containerClient.GetBlobBatchClient();
            var blobStorageUri = blobBatchClient.Uri;
            List<Uri> imageUris = [];
            foreach(var fileName in fileNames)
            {
                imageUris.Add(new Uri(blobStorageUri + "/" + fileName));
            }
            var responses = await blobBatchClient.DeleteBlobsAsync(imageUris, cancellationToken: cancellationToken);
            if (responses.Any(r => r.IsError == true))
            {
                throw new RequestFailedException($"One or more files were not deleted from blob storage. Response message: {string.Join(", ", responses.Select(r => Encoding.UTF8.GetString(r.Content)))}");
            }
        }
        public async Task<string> UploadAsync(IFormFile blob, string fileName, string containerName, CancellationToken cancellationToken = default)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);
            await using(Stream data = blob.OpenReadStream())
            {
                var response = await blobClient.UploadAsync(data, new BlobHttpHeaders()
                {
                    ContentType = blob.ContentType
                }, cancellationToken: cancellationToken);
                using var rawResponse = response.GetRawResponse();
                if (rawResponse.IsError)
                {
                    throw new RequestFailedException($"Request failed. File: {fileName} was not uploaded to blob storage.");
                }
            }
            return blobClient.Uri.AbsolutePath;
        }
        public async Task<BlobStorageDto> DownloadAsync(string fileName, string containerName, CancellationToken cancellationToken = default)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var file = containerClient.GetBlobClient(fileName);
            if(!(await file.ExistsAsync(cancellationToken)))
            {
                throw new RequestFailedException($"Request failed. File: {fileName} was not found.");
            }
            var stream = await file.OpenReadAsync(cancellationToken: cancellationToken);
            var content = await file.DownloadContentAsync(cancellationToken: cancellationToken);
            var contentType = content.Value.Details.ContentType;
            return new BlobStorageDto()
            {
                FileStream = stream,
                ContentType = contentType,
                FileName = fileName
            };
        }
    }
}

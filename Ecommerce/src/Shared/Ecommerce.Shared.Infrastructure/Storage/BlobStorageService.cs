﻿using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Ecommerce.Shared.Abstractions.BloblStorage;
using Ecommerce.Shared.Infrastructure.Storage.Exceptions;
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
        //public async Task DeleteAsync(string fileName, string containerName)
        //{
        //    var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        //    BlobClient blobClient = containerClient.GetBlobClient(fileName);
        //    var a = blobClient.Uri;
        //    using var response = await blobClient.DeleteAsync();
        //    if(response.IsError)
        //    { 
        //        throw new BlobStorageFileNotDeletedException(fileName);
        //    }
        //}
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
            var responses = await blobBatchClient.DeleteBlobsAsync(imageUris);
            if (responses.Any(r => r.IsError == true))
            {
                throw new BlobStorageFilesNotAllDeletedException(fileNames);
            }
        }
        public async Task<string> UploadAsync(IFormFile blob, string fileName, string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            await using(Stream data = blob.OpenReadStream())
            {
                var response = await blobClient.UploadAsync(data, new BlobHttpHeaders()
                {
                    ContentType = blob.ContentType
                });
                using var rawResponse = response.GetRawResponse();
                if (rawResponse.IsError)
                {
                    throw new BlobStorageFileNotUploadedException(fileName);
                }
            }
            return blobClient.Uri.AbsolutePath;
        }
    }
}

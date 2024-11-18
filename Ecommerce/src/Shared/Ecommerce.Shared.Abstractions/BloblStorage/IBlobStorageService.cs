﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Abstractions.BloblStorage
{
    public interface IBlobStorageService
    {
        Task<string> UploadAsync(IFormFile blob, string fileName, string containerName);
        Task DeleteManyAsync(IEnumerable<string> fileNames, string containerName);
        Task DeleteAsync(string fileName, string containerName);
        Task<BlobStorageDto> DownloadAsync(string fileName, string containerName);
    }
}

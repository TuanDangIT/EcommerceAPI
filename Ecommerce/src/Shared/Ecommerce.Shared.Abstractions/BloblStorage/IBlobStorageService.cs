using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.Abstractions.BloblStorage
{
    public interface IBlobStorageService
    {
        Task<string> UploadAsync(IFormFile blob, string fileName, string containerName, CancellationToken cancellationToken = default);
        Task DeleteManyAsync(IEnumerable<string> fileNames, string containerName, CancellationToken cancellationToken = default);
        Task DeleteAsync(string fileName, string containerName, CancellationToken cancellationToken = default);
        Task<BlobStorageDto> DownloadAsync(string fileName, string containerName, CancellationToken cancellationToken = default);
    }
}

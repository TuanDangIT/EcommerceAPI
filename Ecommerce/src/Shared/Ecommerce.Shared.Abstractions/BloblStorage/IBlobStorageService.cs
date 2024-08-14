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
        Task<string> UploadAsync(IFormFile blub, string containerName);
        Task DeleteAsync(string fileName, string containerName);
    }
}

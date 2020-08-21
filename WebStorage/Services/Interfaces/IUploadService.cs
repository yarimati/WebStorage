using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebStorage.Services
{
    public interface IUploadService
    {
        Task<string> Upload(IEnumerable<IFormFile> files);
    }
}
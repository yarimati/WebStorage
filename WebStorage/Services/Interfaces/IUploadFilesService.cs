using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebStorage.Services.Interfaces
{
    public interface IUploadService
    {
        Task<string> Upload(IEnumerable<IFormFile> files);
    }
}
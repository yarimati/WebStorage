using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace WebStorage.Services
{
    public class UploadService : IUploadService
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IGuidService _guidService;
        public UploadService(IGuidService guidService,
                        IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
            _guidService = guidService;
        }

        public async Task<string> Upload(IEnumerable<IFormFile> files)
        {
            var uniqueShortFolderName = _guidService.GenShortUniqueName() + @"\";
            string destinationPath = _appEnvironment.ContentRootPath + @"\Files\";

            try
            {
                foreach (var file in files)
                {
                    if (file != null)
                    {
                        Directory.CreateDirectory(destinationPath + uniqueShortFolderName);

                        var typeFile = Path.GetExtension(file.FileName);
                        var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                        string filePath = destinationPath + uniqueShortFolderName + fileName + typeFile;

                        using var stream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
                        await file.CopyToAsync(stream);
                    }
                }
                return uniqueShortFolderName;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}

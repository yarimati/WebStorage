using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;

namespace WebStorage.Services
{
    public class DeleteFilesService : IDeleteFilesService
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private const int expiresTime = -10;
        public DeleteFilesService(IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }
        public void CheckToDeleteFolder()
        {
            string path = _appEnvironment.ContentRootPath + @"\Files\";

            var folders = Directory.GetDirectories(path);

            foreach (var folder in folders)
            {
                if (GetFolderCreationTime(folder) < DateTime.Now.AddDays(expiresTime))
                {
                    Directory.Delete(folder, true);
                }
            }
        }
        private DateTime GetFolderCreationTime(string path)
        {
            FileInfo fi = new FileInfo(path);

            return fi.CreationTime;
        }
    }
}

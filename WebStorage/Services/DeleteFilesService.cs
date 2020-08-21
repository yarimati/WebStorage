using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;

namespace WebStorage.Services
{
    public class DeleteFilesService : IDeleteFilesService
    {
        private readonly IWebHostEnvironment _appEnvironment;

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
                FileInfo fi = new FileInfo(folder);
                if (fi.CreationTime < DateTime.Now.AddMinutes(-5))
                {
                    Directory.Delete(folder, true);
                }
            }
        }
    }
}

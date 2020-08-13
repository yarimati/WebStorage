using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WebStorage.Services;

namespace WebStorage.Controllers
{
    public class FileController : Controller
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IGuidService _guidService;
        private readonly ICounterFilesService _counterFiles;
        private readonly IDeleteFilesService _deleteFiles;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appEnvironment"></param>
        /// <param name="guidService"></param>
        /// <param name="counterFiles"></param>
        /// <param name="deleteFiles"></param>
        public FileController(IWebHostEnvironment appEnvironment, IGuidService guidService,
                           ICounterFilesService counterFiles, IDeleteFilesService deleteFiles)
        {
            _appEnvironment = appEnvironment;
            _guidService = guidService;
            _counterFiles = counterFiles;
            _deleteFiles = deleteFiles;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Upload()
        {
            ViewData["CurrentCounter"] = _counterFiles.GetCurrentCounterFiles();
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Upload(IEnumerable<IFormFile> files)
        {
            var uniqeShortFolderName = _guidService.GenShortUniqueName();
            string destinationPath = _appEnvironment.ContentRootPath + @"\Files\";
            uniqeShortFolderName = uniqeShortFolderName + @"\";
            foreach (var file in files)
            {
                if (file != null)
                {
                    Directory.CreateDirectory(destinationPath + uniqeShortFolderName);

                    var typeFile = Path.GetExtension(file.FileName);
                    var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string filePath = destinationPath + uniqeShortFolderName + fileName + typeFile;

                    using var stream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
                    await file.CopyToAsync(stream);
                }
            }
            _counterFiles.IncreaseTotalCounter();
            ViewData["TotalCounter"] = _counterFiles.GetTotalCounterFiles();
            _deleteFiles.CheckToDeleteFolder();

            return RedirectToAction("GetAllFiles", new { downloadUrl = uniqeShortFolderName });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="downloadUrl"></param>
        /// <returns></returns>
        public IActionResult GetAllFiles(string downloadUrl)
        {
            var folderPath = _appEnvironment.ContentRootPath + @"\Files\" + downloadUrl;
            DirectoryInfo directory = new DirectoryInfo(folderPath);

            if (!directory.Exists)
                return RedirectToAction("Upload");

            string[] files = Directory.GetFiles(folderPath, "*.*");

            Dictionary<string, string> fileNames = new Dictionary<string, string>();

            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                fileNames.Add(fileName, downloadUrl);
            }
            return View(fileNames);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="uniqFolderName"></param>
        /// <returns></returns>
        public IActionResult Download(string fileName, string uniqFolderName)
        {
            if (fileName == null)
                return RedirectToAction("Upload");
            var fileToDownload = _appEnvironment.ContentRootPath + @"\Files\" + uniqFolderName + @"\" + fileName;
            FileStream fs = new FileStream(fileToDownload, FileMode.Open);
            string file_type = MimeTypes.GetMimeType(fileToDownload);
            return File(fs, file_type, fileName);
        }
    }
}

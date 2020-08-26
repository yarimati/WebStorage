using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebStorage.Models;
using WebStorage.Services;

namespace WebStorage.Controllers
{
    [Authorize]
    public class FileController : Controller
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IDeleteFilesService _deleteFiles;
        private readonly IUploadService _uploadService;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppIdentityDbContext _context;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="appEnvironment"></param>
        /// <param name="counterFiles"></param>
        /// <param name="deleteFiles"></param>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        /// <param name="uploadService"></param>
        public FileController(IWebHostEnvironment appEnvironment,
                            IDeleteFilesService deleteFiles, AppIdentityDbContext context,
                            UserManager<AppUser> userManager, IUploadService uploadService)
        {
            _appEnvironment = appEnvironment;
            _deleteFiles = deleteFiles;
            _context = context;
            _userManager = userManager;
            _uploadService = uploadService;
        }

        /// <summary>
        /// If user comes(~/File) will be redirect to upload action
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return RedirectToAction("Upload");
        }

        /// <summary>
        /// Upload view
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Upload()
        {
            return View();
        }

        /// <summary>
        /// Upload files to the folder
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Upload(IEnumerable<IFormFile> files)
        {
            string download = await _uploadService.Upload(files);

            await AddLinks(await _userManager.GetUserAsync(HttpContext.User), download.Split('/').Last()); //add links in db

            return RedirectToAction("GetFiles", new { downloadUrl = download });
        }

        /// <summary>
        /// Add links to each user in db
        /// </summary>
        /// <param name="user"></param>
        /// <param name="linkToDb"></param>
        /// <returns></returns>

        [HttpPost]
        [Authorize]
        public async Task<bool> AddLinks(AppUser user, string linkToDb)
        {
            if (user != null && linkToDb != null)
            {
                var date = DateTime.Now;
                await _context.Links.AddAsync(new UserLink() { Link = linkToDb, Date = date, AppUser = user });
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Shows files belonging to one user
        /// </summary>
        /// <param name="downloadUrl"></param>
        /// <returns></returns>

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetFiles(string downloadUrl)
        {
            if (downloadUrl == null)
            {
                return RedirectToAction("Upload");
            }

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
        /// Download selected file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="uniqueFolderName"></param>
        /// <returns></returns>

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Download(string fileName, string uniqueFolderName)
        {
            if (fileName == null || uniqueFolderName==null)
                return RedirectToAction("Upload");

            try
            {
                var fileToDownload = _appEnvironment.ContentRootPath + @"\Files\" + uniqueFolderName + @"\" + fileName;
                FileStream fs = new FileStream(fileToDownload, FileMode.Open);
                string file_type = MimeTypes.GetMimeType(fileToDownload);
                return File(fs, file_type, fileName);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new NotFoundResult();
        }
    }
}

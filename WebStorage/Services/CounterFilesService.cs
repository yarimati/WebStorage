using Microsoft.AspNetCore.Hosting;
using System.IO;
using WebStorage.Services.Interfaces;

namespace WebStorage.Services
{
    public class CounterFilesService : ICounterFilesService // not using in project
    {
        private int _currentCounter = 0;
        private int _totalCounter = 0;
        private readonly IWebHostEnvironment _appEnvironment;

        public CounterFilesService(IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
            CountFiles();
        }
        private void CountFiles()
        {
            string path = _appEnvironment.ContentRootPath + @"\Files\";

            var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                _currentCounter++;
            }
        }
        public int GetCurrentCounterFiles() => _currentCounter;
        public int GetTotalCounterFiles() => _totalCounter;
        public void IncreaseTotalCounter() => _totalCounter++;
    }
}

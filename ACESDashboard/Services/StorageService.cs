using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ACESDashboard.Services
{
    public class StorageService : IStorageService
    {
        public StorageService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
            AssetPath = Path.Combine(WebHostEnvironment.WebRootPath, "Assets");            
            PathSeparator = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "\\" : "/";
        }

        public string AssetPath { get; set; }

        public string PathSeparator { get; set; }

        public IWebHostEnvironment WebHostEnvironment { get; }

        public void DeleteFile(string filePath)
        {
            if(VerifyPathValidity(filePath))
                File.Delete(filePath);
        }

        public void DeleteFiles(string[] filePaths)
        {
            foreach (var path in filePaths)
            {
                if (VerifyPathValidity(path))
                    File.Delete(path);
            }
        }
        public async Task<string> SaveFile(IFormFile file)
        {
            string fileName = Guid.NewGuid().ToString();

            string fileDestinationPath = Path.Combine(AssetPath, fileName);
            using (Stream fileStream = new FileStream(fileDestinationPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return fileName;
        }

        public async Task<List<string>> SaveFiles(IFormFile[] files)
        {
            var fileNames = new List<string>();

            for (int i = 0; i < files.Length; i++)
            {
                fileNames[i] = Guid.NewGuid().ToString();
            }

            for (int i = 0; i < files.Length; i++)
            {
                string fileDestinationPath = Path.Combine(AssetPath, fileNames[i]);
                
                using (Stream fileStream = new FileStream(fileDestinationPath, FileMode.Create))
                {
                    await files[i].CopyToAsync(fileStream);
                }
            }

            return fileNames;
        }

        // TODO: Implement
        private bool VerifyPathValidity(string filePath)
        {
            return true;
        }
    }
}
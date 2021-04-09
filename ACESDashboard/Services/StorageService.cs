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

            if (!Directory.Exists(AssetPath))
                Directory.CreateDirectory(AssetPath);

            PathSeparator = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "\\" : "/";
        }

        public string AssetPath { get; set; }

        public string PathSeparator { get; set; }

        public IWebHostEnvironment WebHostEnvironment { get; }

        public void DeleteFile(string fileName)
        {
            string filePath = Path.Combine(AssetPath, fileName);
            if(VerifyPathValidity(filePath))
                File.Delete(filePath);
        }

        public void DeleteFiles(string[] fileNames)
        {
            foreach (var fileName in fileNames)
            {
                string filePath = Path.Combine(AssetPath, fileName);
                if (VerifyPathValidity(filePath))
                    File.Delete(filePath);
            }
        }
        public async Task<string> SaveFile(IFormFile file, string extension)
        {            
            string fileName = $"{Guid.NewGuid()}{extension}";

            string fileDestinationPath = Path.Combine(AssetPath, fileName);
            using (Stream fileStream = new FileStream(fileDestinationPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return fileName;
        }

        public async Task<List<string>> SaveFiles(IFormFile[] files, string[] extensions)
        {
            var fileNames = new List<string>();

            for (int i = 0; i < files.Length; i++)
            {
                fileNames[i] = $"{Guid.NewGuid()}{extensions[i]}";
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

        public byte[] GetFile(string fileName)
        =>  File.ReadAllBytes(Path.Combine(AssetPath, fileName));
        

        private bool VerifyPathValidity(string filePath)
        {
            if (File.Exists(filePath))
                return true;

            throw new FileNotFoundException("File does not exist");
        }
    }
}
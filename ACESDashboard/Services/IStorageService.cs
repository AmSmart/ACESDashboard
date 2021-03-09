using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACESDashboard.Services
{
    public interface IStorageService
    {
        void DeleteFile(string filePath);
        void DeleteFiles(string[] filePaths);
        Task<string> SaveFile(IFormFile file);
        Task<List<string>> SaveFiles(IFormFile[] files);
    }
}

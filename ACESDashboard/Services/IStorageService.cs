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
        void DeleteFile(string fileName);
        void DeleteFiles(string[] fileNames);
        byte[] GetFile(string fileName);
        Task<string> SaveFile(IFormFile file, string extension);
        Task<List<string>> SaveFiles(IFormFile[] files, string[] extensions);
    }
}

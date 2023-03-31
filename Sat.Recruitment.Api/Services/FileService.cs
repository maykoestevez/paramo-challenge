using System.IO;
using Microsoft.Extensions.Configuration;
using Sat.Recruitment.Api.Models;

namespace Sat.Recruitment.Api.Services
{
    /// <summary>
    /// Manage reading user data from file
    /// </summary>
    public class FileService : IFileService
    {
        private readonly string filePath;

        public FileService(IConfiguration configuration)
        {
            filePath = configuration["UserFile"];
        }
        
        public StreamReader ReadUsersFromFile()
        {
            var path = $"{Directory.GetCurrentDirectory()}{filePath}";
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"{Messages.FileNotFoundError}{path}");
            }
            var fileStream = new FileStream(path, FileMode.Open);
            var readerResult = new StreamReader(fileStream);
            
            return readerResult;
        }
    }
}
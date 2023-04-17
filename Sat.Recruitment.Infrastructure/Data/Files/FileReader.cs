using System.IO;
using Sat.Recruitment.Infrastructure.Helpers;
using Sat.Recruitment.Infrastructure.Interfaces;

namespace Sat.Recruitment.Infrastructure.Files
{
    public class FileReader : IFileReader
    {
        public StreamReader ReadFile(string filePath)
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
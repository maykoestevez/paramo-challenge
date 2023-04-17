using System.IO;

namespace Sat.Recruitment.Infrastructure.Interfaces
{
    /// <summary>
    /// Manage all task regarding reading files
    /// </summary>
    public interface IFileReader
    {
        /// <summary>
        /// Return stream from path provide
        /// </summary>
        public StreamReader ReadFile(string filePath);
    }
}
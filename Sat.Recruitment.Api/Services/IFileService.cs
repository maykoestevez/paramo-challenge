using System.IO;

namespace Sat.Recruitment.Api.Services
{
    /// <summary>
    /// Manage files needed to load users
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Return reader with user data
        /// </summary>
        /// <returns></returns>
        public StreamReader ReadUsersFromFile();
    }
}
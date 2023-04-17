using System;
using Sat.Recruitment.Infrastructure.Helpers;

namespace Sat.Recruitment.Infrastructure
{
    public static class Helper
    {
        
        /// <summary>
        /// Normalize email checking for invalid character and removing it
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static string NormalizeEmail(string email)
        {
            if (!email.Contains("@")) throw new ArgumentException(Messages.InvalidEmail);
            
            var emailSplit = email.Split(new char[] {'@'}, StringSplitOptions.RemoveEmptyEntries);
            var indexToRemove = emailSplit[0].IndexOf("+", StringComparison.Ordinal);
            emailSplit[0] = indexToRemove < 0
                ? emailSplit[0].Replace(".", "")
                : emailSplit[0].Replace(".", "").Replace("+","");

            return string.Join("@", new string[] {emailSplit[0], emailSplit[1]});
        }
    }
}
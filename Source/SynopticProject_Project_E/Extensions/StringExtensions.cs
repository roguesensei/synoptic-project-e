using System;
using System.Text;

namespace SynopticProject_Project_E.Extensions
{
    /// <summary>
    /// Class of string extensions
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Converts a string to Base64
        /// </summary>
        /// <param name="source">The string source</param>
        /// <returns>Base64 string</returns>
        public static string ToBase64(this string source)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(source);

            return Convert.ToBase64String(buffer);
        }
    }
}

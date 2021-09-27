using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynopticProject_Project_E.Extensions
{
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

using Microsoft.Extensions.Configuration;
using SynopticProject_Project_E.Models;
using System;

namespace SynopticProject_Project_E.Helpers
{
    /// <summary>
    /// AppSettings Object retreived from appsettings.json
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// MongoDB Connection String
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Name of the Database to read/write collections from/to
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// The default super user object, created if no admin exists
        /// </summary>
        public UserUploadModel DefaultSuperUser { get; set; }
    }

    public static class ConfigurationHelper
    {
        /// <summary>
        /// Gets AppSettings object from appsettings.json
        /// </summary>
        /// <returns>AppSettings Object</returns>
        public static AppSettings GetAppSettings()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            var appSettings = config.GetSection(nameof(AppSettings))
                .Get<AppSettings>();

            return appSettings;
        }
    }
}

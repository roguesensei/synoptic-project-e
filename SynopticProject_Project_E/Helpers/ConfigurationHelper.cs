using Microsoft.Extensions.Configuration;
using System;

namespace SynopticProject_Project_E.Helpers
{
    public class AppSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public static class ConfigurationHelper
    {
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

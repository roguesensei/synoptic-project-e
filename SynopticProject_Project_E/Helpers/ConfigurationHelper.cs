using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynopticProject_Project_E.Helpers
{
    public class AppSettings
    {
        public string ConnectionString { get; set; }
    }

    public static class ConfigurationHelper
    {
        public static AppSettings GetAppSettings()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            var section = config.GetSection(nameof(AppSettings));
            var appSettings = section.Get<AppSettings>();

            return appSettings;
        }
    }
}

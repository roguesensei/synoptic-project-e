using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace SynopticProject_Project_E
{
    /// <summary>
    /// Class housing entry point
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Standard entry point
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Create Host builder
        /// </summary>
        /// <param name="args">args from Main()</param>
        /// <returns>Default Host Builder</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

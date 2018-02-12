using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SezBot3000v2.Services;
using System;

namespace SezBot3000v2_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");

            Startup startup = new Startup(builder.Build());

            startup.ConfigureServices(services);

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            serviceProvider
                .GetService<ILoggerFactory>()
                .AddConsole(LogLevel.Debug);

            var service = serviceProvider.GetService<IUpdateService>();

            Console.ReadKey();
        }
    }
}

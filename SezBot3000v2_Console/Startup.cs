using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SezBot3000v2;
using SezBot3000v2.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SezBot3000v2_Console
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IBotService, BotService>();
            services.AddScoped<IUpdateService, UpdateServiceConsole>();
            services.AddLogging();
            services.AddOptions();

            services.Configure<BotConfiguration>(Configuration.GetSection("BotConfiguration"));
            services.Configure<BotReplyBank>(Configuration.GetSection("BotReplyBank"));
        }
    }
}

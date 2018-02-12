using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SezBot3000v2.Models;
using SezBot3000v2.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SezBot3000v2_Console
{
    public class UpdateServiceConsole : UpdateService
    {
        private IBotService _botService { get; set; }
        public UpdateServiceConsole(IBotService botService, ILogger<UpdateServiceConsole> logger, IOptions<BotReplyBank> botReplyBank)
            : base(botService, logger, botReplyBank)
        {
            _botService = botService;
            _botService.Client.DeleteWebhookAsync().Wait();
            _botService.Client.OnUpdate += Client_OnUpdate;
            _botService.Client.StartReceiving();
            var me = _botService.Client.GetMeAsync().Result;
            logger.LogInformation($"Bot {me.Username} started");
        }

        private async void Client_OnUpdate(object sender, Telegram.Bot.Args.UpdateEventArgs e)
        {
            await Update(e.Update);
        }
    }
}

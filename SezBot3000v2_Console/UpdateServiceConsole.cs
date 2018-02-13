using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SezBot3000v2.Models;
using SezBot3000v2.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace SezBot3000v2_Console
{
    public class UpdateServiceConsole : IUpdateService
    {
        private IBotService _botService { get; set; }
        public IUpdateService _decoratee { get; private set; }
        public UpdateServiceConsole(IUpdateService decoratee, IBotService botService, ILogger<IUpdateService> logger, IOptions<BotReplyBank> botReplyBank)
        {
            _botService = botService;
            _decoratee = decoratee;
            _botService.Client.DeleteWebhookAsync().Wait();
            _botService.Client.OnUpdate += Client_OnUpdate;
            _botService.Client.StartReceiving();
        }

        public async Task Update(Update update)
        {
            await _decoratee.Update(update);
        }

        public async void Client_OnUpdate(object sender, UpdateEventArgs e)
        {
            await Update(e.Update);
        }
    }
}

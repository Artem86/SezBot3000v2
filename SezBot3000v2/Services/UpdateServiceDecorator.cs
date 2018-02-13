using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SezBot3000v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace SezBot3000v2.Services
{
    public class UpdateServiceDecorator : IUpdateService
    {
        public IUpdateService _decoratee { get; private set; }
        public IBotService _botService { get; private set; }
        public ILogger<IUpdateService> _logger { get; private set; }
        public BotReplyBank _botReplyBank { get; private set; }

        public UpdateServiceDecorator(IUpdateService decoratee, IBotService botService, ILogger<IUpdateService> logger, IOptions<BotReplyBank> botReplyBank)
        {
            _decoratee = decoratee;
            _botService = botService;
            _logger = logger;
            _botReplyBank = botReplyBank.Value;
            var me = _botService.Client.GetMeAsync().Result;
            _logger.LogInformation($"Bot {me.Username} started");
        }
        public async Task Update(Update update)
        {
            var message = update.Message;
            _logger.LogInformation($"Received Message from {message.Chat.FirstName} {message.Chat.LastName} ({message.Chat.Username} - id: {message.Chat.Id})");
            await _decoratee.Update(update);
        }
    }
}

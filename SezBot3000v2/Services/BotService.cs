using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace SezBot3000v2.Services
{
    public class BotService : IBotService
    {
        private readonly BotConfiguration _config;

        public BotService(IOptions<BotConfiguration> config)
        {
            _config = config.Value;
            Client = new TelegramBotClient(_config.BotToken);
            Client.SetWebhookAsync(_config.WebHook).Wait();
        }

        public TelegramBotClient Client { get; }
    }
}

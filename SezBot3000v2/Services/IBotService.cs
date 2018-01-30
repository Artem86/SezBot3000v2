using Telegram.Bot;

namespace SezBot3000v2.Services
{
    public interface IBotService
    {
        TelegramBotClient Client { get; }
    }
}

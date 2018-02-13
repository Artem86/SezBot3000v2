using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SezBot3000v2.Extensions;
using SezBot3000v2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SezBot3000v2.Services
{
    public class UpdateService : IUpdateService
    {
        private readonly IBotService _botService;
        private readonly ILogger<IUpdateService> _logger;
        private BotReplyBank _botReplyBank;

        public UpdateService(IBotService botService, ILogger<IUpdateService> logger, IOptions<BotReplyBank> botReplyBank)
        {
            _botService = botService;
            _logger = logger;
            _botReplyBank = botReplyBank.Value;
        }

        public async Task Update(Update update)
        {
            if (update.Type != UpdateType.MessageUpdate) return;

            var message = update.Message;

            if (!message.HasAnchor(_botReplyBank.ShouldReplyAnchors)) return;

            if (message.Type == MessageType.TextMessage)
            {
                if (message.HasAnchor(_botReplyBank.MusicReplyAnchors) && await SendMusicReply(message)) return;
                await SendStickerReply(message);
                var text = GetTextReply(message.Text);
                await _botService.Client.SendTextMessageAsync(message.Chat.Id, text);
            }
        }

        private string GetTextReply(string message)
        {
            var replyTemplateQuery = _botReplyBank.ContextReply.Where(cr => message.Contains(cr.Key)).SelectMany(cr => cr.Value);
            if (!replyTemplateQuery.Any())
                return _botReplyBank.DefaultReply.GetRandomElement();
            return replyTemplateQuery.GetRandomElement();
        }

        private async Task<bool> SendStickerReply(Message message)
        {
            if (!message.HasAnchor(_botReplyBank.ContextSticker)) return false;

            var replyTemplateQuery = _botReplyBank.ContextSticker.Where(cs => message.Text.Contains(cs.Key)).SelectMany(cr => cr.Value);
            var sticker = replyTemplateQuery.GetRandomElement();
            using (var fs = new FileStream(sticker.Path, FileMode.Open))
            {
                var stickerToSend = fs.ToFileToSend(sticker.Path);
                await _botService.Client.SendStickerAsync(message.Chat.Id, stickerToSend);
                return true;
            }
        }

        private async Task<bool> SendMusicReply(Message message)
        {
            if (!message.HasAnchor(_botReplyBank.ContextMusic)) return false;

            var musicReply = _botReplyBank.ContextMusic
                .Where(cm => message.Text.Contains(cm.Key))
                .SelectMany(cm => cm.Value)
                .GetRandomElement();
            using (var fs = new FileStream(musicReply.Path, FileMode.Open))
            {
                var musicToSend = fs.ToFileToSend(musicReply.Path);
                await _botService.Client.SendAudioAsync(message.Chat.Id, musicToSend, musicReply.Caption, musicReply.Duration, musicReply.Performer, musicReply.Title);
                return true;
            }
        }
    }
}

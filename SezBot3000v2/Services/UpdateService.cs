using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SezBot3000v2.Services
{
    public class UpdateService : IUpdateService
    {
        private readonly IBotService _botService;
        private readonly ILogger<UpdateService> _logger;
        private BotReplyBank _botReplyBank;
        private IEnumerable<string> _anchor;
        private IEnumerable<string> _defaultReplies;
        private IDictionary<string, IEnumerable<string>> _contextReplies;

        public UpdateService(IBotService botService, ILogger<UpdateService> logger, IOptions<BotReplyBank> botReplyBank)
        {
            _botService = botService;
            _logger = logger;
            _botReplyBank = botReplyBank.Value;
            _anchor = _botReplyBank.ShouldReplyAnchors;
            _defaultReplies = _botReplyBank.DefaultReply;
            _contextReplies = _botReplyBank.ContextReply;
        }

        public async Task Update(Update update)
        {
            if (update.Type != UpdateType.MessageUpdate)
            {
                return;
            }

            var message = update.Message;

            _logger.LogInformation("Received Message from {0}", message.Chat.Id);

            if (message.Type == MessageType.TextMessage)
            {
                if (ShouldReply(message.Text))
                {
                    var text = GetReply(message.Text);
                    await _botService.Client.SendTextMessageAsync(message.Chat.Id, text);
                }
                    
            }

            else if (message.Type == MessageType.PhotoMessage)
            {
                // Download Photo
                var fileId = message.Photo.LastOrDefault()?.FileId;
                var file = await _botService.Client.GetFileAsync(fileId);

                var filename = file.FileId + "." + file.FilePath.Split('.').Last();

                using (var saveImageStream = System.IO.File.Open(filename, FileMode.Create))
                {
                    await file.FileStream.CopyToAsync(saveImageStream);
                }

                var sticker = FileToSendExtensions.ToFileToSend(new FileStream("Stickers/frogSticker.webp", FileMode.Open), "frogSticker");
                await _botService.Client.SendStickerAsync(message.Chat.Id, sticker);
                    ////SendTextMessageAsync(message.Chat.Id, "пикча норм");
            }
        }

        private bool ShouldReply(string message)
        {
            var words = message.ToLower().Split(' ').ToList();
            words.ForEach(word => word.Replace(" ", ""));
            return words.Any(word => _anchor.Any(a => word.Contains(a)));
        }

        private string GetReply(string message)
        {
            var replyTemplateQuery = _contextReplies.Where(_ => message.Contains(_.Key)).SelectMany(_ => _.Value);
            var r = new Random(DateTime.Now.Millisecond);
            if (!replyTemplateQuery.Any())
                return _defaultReplies.ElementAt(r.Next(0, _defaultReplies.Count()));
            var replyPosition = r.Next(0, replyTemplateQuery.Count());
            return replyTemplateQuery.ElementAt(replyPosition);
        }

    }
}

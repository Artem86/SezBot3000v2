using Microsoft.Extensions.Logging;
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
        private IEnumerable<string> _anchor = new List<string> {
            "сез",
            "sez",
            "cеня",
            "сезон",
            "сезом",
            "сеза",
            "сезу",
            "бот",
            "bot"
        };
        private IEnumerable<string> _defaultReplies = new List<string>
        {
            "Я устаю пздц",
            "Дрисня",
            "Я хил",
            "Ну почему",
            "Непонятно",
            "Я качаю неспеша",
            "Го гирзы",
            "Го дьябла",
            "Го ваха",
            "Когда го?",
            "Го катать ?",
            "Во сколько сегодня катаем?",
            "Я на Работке",
            "Объясните",
            "Ну почему вы катаете в жало?",
            "Заебали))",
            "У меня мало места пздц",
            "Я надеялся про вопрос о сиськах миры",
            "Вы льётесь с каток"
        };
        private IEnumerable<KeyValuePair<string, string>> _replyTemplates = new List<KeyValuePair<string, string>> {
            new KeyValuePair<string, string>("катку", "вы всё равно катаете в жало"),
            new KeyValuePair<string, string>("дьябл", "дьябла норм"),
            new KeyValuePair<string, string>("гирз", "гирзы норм"),
            new KeyValuePair<string, string>("катать", "вы всё равно катаете в жало"),
            new KeyValuePair<string, string>("каточку", "вы всё равно катаете в жало"),
        };

        public UpdateService(IBotService botService, ILogger<UpdateService> logger)
        {
            _botService = botService;
            _logger = logger;
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

                await _botService.Client.SendTextMessageAsync(message.Chat.Id, "пикча норм");
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
            var replyTemplateQuery = _replyTemplates.Where(_ => message.Contains(_.Key));
            var r = new Random(DateTime.Now.Millisecond);
            if (!replyTemplateQuery.Any())
                return _defaultReplies.ElementAt(r.Next(0, _defaultReplies.Count()));
            var replyPosition = r.Next(0, replyTemplateQuery.Count());
            return replyTemplateQuery.ElementAt(replyPosition).Value;
        }

    }
}

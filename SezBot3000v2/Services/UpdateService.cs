using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SezBot3000v2.Extensions;
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

        public UpdateService(IBotService botService, ILogger<UpdateService> logger, IOptions<BotReplyBank> botReplyBank)
        {
            _botService = botService;
            _logger = logger;
            _botReplyBank = botReplyBank.Value;
        }

        public async Task Update(Update update)
        {
            if (update.Type != UpdateType.MessageUpdate)
            {
                return;
            }

            var message = update.Message;

            _logger.LogInformation("Received Message from {0}", message.Chat.Id);

            if (!message.HasAnchor(_botReplyBank.ShouldReplyAnchors)) return;

            if (message.Type == MessageType.TextMessage)
            {
                //if (await SendMusicReply(message))
                //{
                //    return;
                //}
                //await SendStickerReply(message);
                var text = GetTextReply(message.Text);
                await _botService.Client.SendTextMessageAsync(message.Chat.Id, text);
            }
            
            //else if (message.Type == MessageType.PhotoMessage)
            //{
            //    // Download Photo
            //    var fileId = message.Photo.LastOrDefault()?.FileId;
            //    var file = await _botService.Client.GetFileAsync(fileId);

            //    var filename = file.FileId + "." + file.FilePath.Split('.').Last();

            //    using (var saveImageStream = System.IO.File.Open(filename, FileMode.Create))
            //    {
            //        await file.FileStream.CopyToAsync(saveImageStream);
            //    }
            //        ////SendTextMessageAsync(message.Chat.Id, "пикча норм");
            //}
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
            string stickerPath = replyTemplateQuery.GetRandomElement();
            using (var fs = new FileStream(stickerPath, FileMode.Open))
            {
                var stickerToSend = FileToSendExtensions.ToFileToSend(fs, "frogsticker");
                await _botService.Client.SendStickerAsync(message.Chat.Id, stickerToSend);
                return true;
            }
        }

        private async Task<bool> SendMusicReply(Message message)
        {
            if (!message.HasAnchor(_botReplyBank.ContextMusic)) return false;

            var parameters = _botReplyBank.ContextMusic.Where(cm => message.Text.Contains(cm.Key)).SelectMany(cm => cm.Value);
            string musicPath = parameters.ElementAt(0);
            string comment = parameters.ElementAt(1);
            int SongTime = 0;
            int.TryParse(parameters.ElementAt(2), out SongTime);
            string artist = parameters.ElementAt(3);
            string song = parameters.ElementAt(4);
            using (var fs = new FileStream(musicPath, FileMode.Open))
            {
                var musicToSend = FileToSendExtensions.ToFileToSend(fs, "music");
                await _botService.Client.SendAudioAsync(message.Chat.Id, musicToSend, comment, SongTime, artist, song);
                return true;
            }
        }
       

        ///uncomment for test purposes
        //public async Task Test()
        //{
        //    var parameters = _botReplyBank.ContextMusic.Where(cm => cm.Value.Contains("кухн")).SelectMany(cm => cm.Value);
        //    string musicPath = parameters.ElementAt(0);
        //    string comment = parameters.ElementAt(1);
        //    int SongTime = 0;
        //    int.TryParse(parameters.ElementAt(2), out SongTime);
        //    string artist = parameters.ElementAt(3);
        //    string song = parameters.ElementAt(4);
        //    FileStream fs = new FileStream(musicPath, FileMode.Open);
        //    var musicToSend = FileToSendExtensions.ToFileToSend(fs, "music");
        //    await _botService.Client.SendAudioAsync(310954670, musicToSend, comment, SongTime, artist, song);
        //}
    }
}

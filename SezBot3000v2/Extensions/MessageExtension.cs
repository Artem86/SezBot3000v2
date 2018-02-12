using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace SezBot3000v2.Extensions
{
    public static class MessageExtension
    {
        public static bool HasAnchor<T>(this Message message, IDictionary<string, IEnumerable<T>> collection)
        {
            return message.SplitByWords().Any(word => collection.Keys.Any(a => word.Contains(a)));
        }

        public static bool HasAnchor(this Message message, IEnumerable<string> collection)
        {
            return message.SplitByWords().Any(word => collection.Any(a => word.Contains(a)));
        }

        public static IEnumerable<string> SplitByWords(this Message message)
        {
            var words = message.Text.ToLower().Split(' ').ToList();
            words.ForEach(word => word.Replace(" ", ""));
            return words;
        }
    }
}

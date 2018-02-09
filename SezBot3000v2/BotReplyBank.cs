﻿using System.Collections.Generic;

namespace SezBot3000v2
{
    public class BotReplyBank
    {
        public IEnumerable<string> ShouldReplyAnchors { get; set; }
        public IEnumerable<string> DefaultReply { get; set; }
        public IDictionary<string, IEnumerable<string>> ContextReply { get; set; }
    }
}

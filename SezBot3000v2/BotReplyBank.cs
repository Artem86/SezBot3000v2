﻿using System.Collections.Generic;

namespace SezBot3000v2
{
    public class BotReplyBank
    {
        public IEnumerable<string> ShouldReplyAnchors;
        public IEnumerable<string> DefaultReply;
        public IEnumerable<KeyValuePair<string, string>> ContextReply;
    }
}
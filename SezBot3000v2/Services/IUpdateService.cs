﻿using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace SezBot3000v2.Services
{
    public interface IUpdateService
    {
        Task Update(Update update);
    }
}

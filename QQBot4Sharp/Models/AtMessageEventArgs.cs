using QQBot.NET.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QQBot.NET.Models
{
    /// <summary>
    /// At消息事件参数
    /// </summary>
    public class AtMessageEventArgs : GuildMessageEventArgs
    {
        internal AtMessageEventArgs(BotContext botContext, GuildMessage message) : base(botContext, message)
        {
        }
    }
}

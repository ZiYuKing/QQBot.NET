using QQBot.NET.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace QQBot.NET.Models
{
    /// <summary>
    /// 消息表态事件参数
    /// </summary>
    public class MessageReactionEventArgs : ContextEventArgs
    {
        /// <summary>
        /// 消息表态
        /// </summary>
        public GuildMessageReaction MessageReaction { get; }

        internal MessageReactionEventArgs(BotContext botContext, GuildMessageReaction messageReaction) : base(botContext)
        {
            MessageReaction = messageReaction;
        }
    }
}

using QQBot.NET.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace QQBot.NET.Models
{
    /// <summary>
    /// 交互事件参数
    /// </summary>
    public class InteractionEventArgs : ContextEventArgs
    {
        /// <summary>
        /// 交互信息
        /// </summary>
        public Interaction Interaction { get; }

        internal InteractionEventArgs(BotContext botContext, Interaction interaction) : base(botContext)
        {
            Interaction = interaction;
        }
    }
}

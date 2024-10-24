using QQBot.NET.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace QQBot.NET.Models
{
	/// <summary>
	/// 频道删除事件参数
	/// </summary>
	public class GuildDeleteEventArgs : ContextEventArgs
	{
		/// <summary>
		/// 频道事件信息
		/// </summary>
		public GuildEventData Guild { get; }

		internal GuildDeleteEventArgs(BotContext botContext, GuildEventData guild) : base(botContext)
		{
			Guild = guild;
		}
	}
}

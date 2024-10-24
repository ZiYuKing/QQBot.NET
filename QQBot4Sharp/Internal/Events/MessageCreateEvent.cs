using QQBot.NET.Internal.API;
using QQBot.NET.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QQBot.NET.Internal.Events
{
    internal class MessageCreateEvent : Event
	{
		public override async Task HandleAsync(Payload payload)
		{
			if (payload.OpCode != OpCode.Dispatch)
			{
				return;
			}
			if (payload.Type != "MESSAGE_CREATE")
			{
				return;
			}

			await BotService.SendCreateMessageEventAsync(new(BotContext, payload.Cast<GuildMessage>().Data));
		}
	}
}

using QQBot.NET.Internal.API;
using QQBot.NET.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QQBot.NET.Internal.Events
{
    internal class AtMessageCreateEvent : Event
	{
		public override async Task HandleAsync(Payload payload)
		{
			if (payload.OpCode != OpCode.Dispatch)
			{
				return;
			}
			if (payload.Type != "AT_MESSAGE_CREATE")
			{
				return;
			}

			await BotService.SendAtMessageCreateEventAsync(new(BotContext, payload.Cast<GuildMessage>().Data));
		}
	}
}

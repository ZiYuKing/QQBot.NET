using QQBot.NET.Internal.API;
using QQBot.NET.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QQBot.NET.Internal.Events
{
    internal class MessageReactionAddEevnt : Event
	{
		public override async Task HandleAsync(Payload payload)
		{
			if (payload.OpCode != OpCode.Dispatch)
			{
				return;
			}
			if (payload.Type != "MESSAGE_REACTION_ADD")
			{
				return;
			}

			await BotService.SendMessageReactionAddEventAsync(new(BotContext, payload.Cast<GuildMessageReaction>().Data));
		}
	}
}

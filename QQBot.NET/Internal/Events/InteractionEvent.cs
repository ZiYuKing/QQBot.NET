using QQBot.NET.Internal.API;
using QQBot.NET.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QQBot.NET.Internal.Events
{
    internal class InteractionEvent : Event
	{
		public override async Task HandleAsync(Payload payload)
		{
			if (payload.OpCode != OpCode.Dispatch)
			{
				return;
			}
			if (payload.Type != "INTERACTION_CREATE")
			{
				return;
			}

			await BotService.SendInteractionCreateEventAsync(new(BotContext, payload.Cast<Interaction>().Data));
		}
	}
}

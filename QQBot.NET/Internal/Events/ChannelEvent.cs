using QQBot.NET.Internal.API;
using QQBot.NET.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QQBot.NET.Internal.Events
{
	internal class ChannelEvent : Event
	{
		public override async Task HandleAsync(Payload payload)
		{
			if (payload.OpCode != OpCode.Dispatch)
			{
				return;
			}

			switch (payload.Type)
			{
				case "CHANNEL_CREATE":
					await BotService.SendChannelCreateEventAsync(new(BotContext, payload.Cast<ChannelEventData>().Data));
					break;
				case "CHANNEL_UPDATE":
					await BotService.SendChannelUpdateEventAsync(new(BotContext, payload.Cast<ChannelEventData>().Data));
					break;
				case "CHANNEL_DELETE":
					await BotService.SendChannelDeleteEventAsync(new(BotContext, payload.Cast<ChannelEventData>().Data));
					break;
				default:
					break;
			}
		}
	}
}

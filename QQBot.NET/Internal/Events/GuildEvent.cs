﻿using QQBot.NET.Internal.API;
using QQBot.NET.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QQBot.NET.Internal.Events
{
	internal class GuildEvent : Event
	{
		public override async Task HandleAsync(Payload payload)
		{
			if (payload.OpCode != OpCode.Dispatch)
			{
				return;
			}

			switch (payload.Type)
			{
				case "GUILD_CREATE":
					await BotService.SendGuildCreateEventAsync(new(BotContext, payload.Cast<GuildEventData>().Data));
					break;
				case "GUILD_UPDATE":
					await BotService.SendGuildUpdateEventAsync(new(BotContext, payload.Cast<GuildEventData>().Data));
					break;
				case "GUILD_DELETE":
					await BotService.SendGuildDeleteEventAsync(new(BotContext, payload.Cast<GuildEventData>().Data));
					break;
				default:
					break;
			}
		}
	}
}

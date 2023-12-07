﻿using QQBot4Sharp.Internal.API;
using QQBot4Sharp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QQBot4Sharp.Internal.Events
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

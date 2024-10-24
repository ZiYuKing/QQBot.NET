using Newtonsoft.Json.Linq;
using QQBot.NET.Internal.API;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QQBot.NET.Internal.Events
{
	internal class ReadyEvent : Event
	{
		public ReadyEvent()
		{

		}

		public override async Task HandleAsync(Payload payload)
		{
			if (payload.OpCode != OpCode.Dispatch)
			{
				return;
			}
			if (payload.Type != "READY")
			{
				return;
			}

			await OnReady(payload.Cast<ReadyRes>());

			await Task.CompletedTask;
		}

		private async Task OnReady(Payload<ReadyRes> payload)
		{
			Log.Information("鉴权成功");

			EventBus.Get<HeartbeatEvent>().Start(payload.Data.SessionID);

			await BotService.SendReadyEventAsync(new(BotContext));
		}
	}
}

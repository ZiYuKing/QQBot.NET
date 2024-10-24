using QQBot.NET.Internal.API;
using System;
using System.Threading.Tasks;

namespace QQBot.NET.Internal.Events
{
    internal abstract class Event : IDisposable
	{
		public EventBus EventBus { get; internal set; }

		protected BotWebSocket BotWebSocket => EventBus.BotWebSocket;

		protected BotContext BotContext => EventBus.BotWebSocket.BotContext;

		protected BotService BotService => EventBus.BotWebSocket.BotContext.Service;

		public Event()
		{

		}

		public abstract Task HandleAsync(Payload payload);

		public virtual void Dispose()
		{

		}
	}
}

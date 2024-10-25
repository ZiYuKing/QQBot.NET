using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace QQBot.NET.Internal.API
{
	internal struct HelloRes
	{
		[JsonProperty("heartbeat_interval")]
		public int HeartbeatInterval;
	}
}

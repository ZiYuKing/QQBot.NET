using Newtonsoft.Json;
using QQBot.NET.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QQBot.NET.Internal.API
{
    internal struct ReadyRes
	{
		[JsonProperty("version")]
		public int Version;

		[JsonProperty("session_id")]
		public string SessionID;

		[JsonProperty("user")]
		public GuildUser User;

		[JsonProperty("shard")]
		public List<int> Shard;
	}
}

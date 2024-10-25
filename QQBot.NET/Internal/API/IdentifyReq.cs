using Newtonsoft.Json;
using QQBot.NET.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QQBot.NET.Internal.API
{
	internal struct IdentifyReq
	{
		[JsonProperty("token")]
		public string Token;

		[JsonProperty("intents")]
		public Intents Intents;

		[JsonProperty("shard")]
		public List<int> Shard;

		[JsonProperty("properties")]
		public Dictionary<string, string> Properties;
	}
}

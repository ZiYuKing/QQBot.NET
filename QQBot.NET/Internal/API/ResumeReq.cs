using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace QQBot.NET.Internal.API
{
	internal struct ResumeReq
	{
		[JsonProperty("token")]
		public string Token { get; set; }

		[JsonProperty("session_id")]
		public string SessionID { get; set; }

		[JsonProperty("seq")]
		public int? Seq { get; set; }
	}
}

﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace QQBot.NET.Internal.API
{
	internal struct GatewayRes
	{
		[JsonProperty("url")]
		public string Url;

		public override string ToString()
		{
			return $"Url={Url}";
		}
	}
}

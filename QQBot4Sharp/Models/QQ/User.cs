﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace QQBot4Sharp.Models.QQ
{
	/// <summary>
	/// QQ消息发送者
	/// </summary>
	public class User
	{
		/// <summary>
		/// 用户OpenID
		/// </summary>
		[JsonProperty("user_openid")]
		public string ID { get; set; }
	}
}

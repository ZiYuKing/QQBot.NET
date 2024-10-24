using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace QQBot.NET.Models
{
    /// <summary>
    /// QQ消息发送者
    /// </summary>
    public class QQUser
    {
        /// <summary>
        /// 用户OpenID
        /// </summary>
        [JsonProperty("user_openid")]
        public string ID { get; set; }
    }
}

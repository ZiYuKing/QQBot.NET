﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace QQBot.NET.Models
{
    /// <summary>
    /// QQ私聊消息结果
    /// </summary>
    public class QQMessageRes
    {
        /// <summary>
        /// 消息唯一ID
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }
    }
}

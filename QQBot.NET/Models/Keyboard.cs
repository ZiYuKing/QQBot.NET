﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace QQBot.NET.Models
{
    /// <summary>
    /// 消息交互
    /// </summary>
    public class Keyboard
    {
        /// <summary>
        /// 消息交互内容
        /// </summary>
        [JsonProperty("content")]
        public KeyboardContent Content;
    }
}

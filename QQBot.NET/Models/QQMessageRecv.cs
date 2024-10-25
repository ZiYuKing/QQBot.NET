﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace QQBot.NET.Models
{
    /// <summary>
    /// 接收到的QQ消息
    /// </summary>
    public class QQMessageRecv
    {
        /// <summary>
        /// 消息唯一ID
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// 发送者
        /// </summary>
        [JsonProperty("author")]
        public QQUser Author { get; set; }

        /// <summary>
        /// 发送 QQ 群 ID
        /// </summary>
        [JsonProperty("group_openid")]
        public string Group_OpenID { get; set; }

        /// <summary>
        /// 文本消息内容
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }

        /// <summary>
        /// 消息生产时间（RFC3339）
        /// </summary>
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 富媒体文件附件，文件类型："图片，语音，视频，文件"
        /// </summary>
        [JsonProperty("attachments")]
        public List<QQAttachment> Attachments { get; set; }
    }
}

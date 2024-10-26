using QQBot.NET.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QQBot.NET.Models.QQ
{
    /// <summary>
    /// QQ单聊用户消息事件参数
    /// </summary>
    public class QQUserMessageEventArgs : ReplyEventArgs<QQMessageRecv, QQMessageReq, QQMessageRes>
	{
		internal QQUserMessageEventArgs(BotContext botContext, QQMessageRecv message) : base(botContext, message)
		{
		}

		/// <inheritdoc/>
		public override async Task<QQMessageRes> ReplyAsync(QQMessageReq req, bool setMessageIDAuto = true)
		{
			if (setMessageIDAuto)
			{
				req.MessageID = Message.ID;
			}
			return await SendUserMessageAsync(req, Message.Author.ID);
		}
    }
}

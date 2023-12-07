﻿using QQBot4Sharp.Exceptions;
using QQBot4Sharp.Internal;
using QQBot4Sharp.Internal.API;
using QQBot4Sharp.Models;
using QQBot4Sharp.Models.Guild;
using QQBot4Sharp.Models.QQ;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace QQBot4Sharp
{
	/// <summary>
	/// 异步事件处理器
	/// </summary>
	/// <typeparam name="TEventArgs">事件参数类型</typeparam>
	/// <param name="sender">发送者</param>
	/// <param name="e">事件参数</param>
	/// <returns>任务</returns>
	public delegate Task AsyncEventHandler<TEventArgs>(object sender, TEventArgs e);

	/// <summary>
	/// 机器人服务，提供机器人的功能支持
	/// </summary>
	public class BotService : IDisposable
	{
		private readonly BotCreateInfo _createInfo;

		internal BotCreateInfo CreateInfo => _createInfo;

		private readonly BotContext _botContext;

		/// <summary>
		/// 创建并返回机器人服务实例
		/// </summary>
		/// <param name="createInfo">Bot服务创建信息</param>
		/// <exception cref="ArgumentNullException"></exception>
		public BotService(BotCreateInfo createInfo)
		{
			if (string.IsNullOrEmpty(createInfo.AppID))
			{
				throw new ArgumentNullException(nameof(createInfo.AppID));
			}
			if (string.IsNullOrEmpty(createInfo.ClientSecret))
			{
				throw new ArgumentNullException(nameof(createInfo.ClientSecret));
			}

			_createInfo = createInfo;
			_botContext = new(this);

			Log.Information($"创建机器人服务：{CreateInfo}");
		}

		#region 通用

		/// <summary>
		/// 启动服务
		/// </summary>
		/// <returns>启动任务</returns>
		public async Task StartAsync()
		{
			Log.Information("正在启动机器人服务");

			await _botContext.StartAsync();
		}

		/// <summary>
		/// 停止服务
		/// </summary>
		/// <returns>停止任务</returns>
		public async Task StopAsync()
		{
			Log.Information("正在停止机器人服务");

			await _botContext.StopAsync();
		}

		#endregion

		#region 事件

		/// <summary>
		/// READY事件（鉴权成功后，QQ 后台会下发一个 Ready Event）
		/// </summary>
		public event AsyncEventHandler<ContextEventArgs> OnReadyAsync;

		internal async Task SendReadyEventAsync(ContextEventArgs e)
		{
			try
			{
				await OnReadyAsync?.Invoke(this, e);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "处理READY事件时发生异常");
			}
		}

		/// <summary>
		/// 文字子频道全量消息（私域）<br/>用户在文字子频道内发送的所有聊天消息（私域）
		/// </summary>
		public event AsyncEventHandler<Models.Guild.MessageEventArgs> OnMessageCreateAsync;

		internal async Task SendCreateMessageEventAsync(Models.Guild.MessageEventArgs e)
		{
			try
			{
				await OnMessageCreateAsync?.Invoke(this, e);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "处理MESSAGE_CREATE事件时发生异常");
			}
		}

		/// <summary>
		/// 文字子频道@机器人<br/>用户在文字子频道内@机器人发送的消息
		/// </summary>
		public event AsyncEventHandler<AtMessageEventArgs> OnAtMessageCreateAsync;

		internal async Task SendAtMessageCreateEventAsync(AtMessageEventArgs e)
		{
			try
			{
				await OnAtMessageCreateAsync?.Invoke(this, e);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "处理AT_MESSAGE_CREATE事件时发生异常");
			}
		}

		/// <summary>
		/// 频道私信消息<br/>用户在频道私信给机器人发送的消息
		/// </summary>
		public event AsyncEventHandler<DirectMessageEventArgs> OnDirectMessageCreateAsync;

		internal async Task SendDirectMessageCreateEventAsync(DirectMessageEventArgs e)
		{
			try
			{
				await OnDirectMessageCreateAsync?.Invoke(this, e);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "处理DIRECT_MESSAGE_CREATE事件时发生异常");
			}
		}

		/// <summary>
		/// 单聊消息<br/>用户在单聊发送消息给机器人
		/// </summary>
		public event AsyncEventHandler<Models.QQ.MessageEventArgs> OnC2CMessageCreateAsync;

		internal async Task SendC2CMessageCreateEventAsync(Models.QQ.MessageEventArgs e)
		{
			try
			{
				await OnC2CMessageCreateAsync?.Invoke(this, e);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "处理C2C_MESSAGE_CREATE事件时发生异常");
			}
		}

		/// <summary>
		/// 群聊@机器人<br/>用户在群内@机器人发动的消息
		/// </summary>
		public event AsyncEventHandler<Models.QQ.MessageEventArgs> OnGroupAtMessageCreateAsync;

		internal async Task SendGroupAtMessageCreateEventAsync(Models.QQ.MessageEventArgs e)
		{
			try
			{
				await OnGroupAtMessageCreateAsync?.Invoke(this, e);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "处理GROUP_AT_MESSAGE_CREATE事件时发生异常");
			}
		}

		/// <summary>
		/// 用户对消息进行表情表态时
		/// </summary>
		public event AsyncEventHandler<MessageReactionEventArgs> OnMessageReactionAddAsync;

		internal async Task SendMessageReactionAddEventAsync(MessageReactionEventArgs e)
		{
			try
			{
				await OnMessageReactionAddAsync?.Invoke(this, e);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "处理MESSAGE_REACTION_ADD事件时发生异常");
			}
		}

		/// <summary>
		/// 用户对消息进行取消表情表态时
		/// </summary>
		public event AsyncEventHandler<MessageReactionEventArgs> OnMessageReactionRemoveAsync;

		internal async Task SendMessageReactionRemoveEventAsync(MessageReactionEventArgs e)
		{
			try
			{
				await OnMessageReactionRemoveAsync?.Invoke(this, e);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "处理MESSAGE_REACTION_REMOVE事件时发生异常");
			}
		}

		/// <summary>
		/// 用户点击了消息体的回调按钮
		/// </summary>
		public event AsyncEventHandler<InteractionEventArgs> OnInteractionCreateAsync;

		internal async Task SendInteractionCreateEventAsync(InteractionEventArgs e)
		{
			try
			{
				await OnInteractionCreateAsync?.Invoke(this, e);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "处理INTERACTION_CREATE事件时发生异常");
			}
		}

		#endregion

		#region API

		/// <summary>
		/// 发送文字子频道消息<br/>注意：如果消息需要审核，会抛出异常，详见 <a href="https://bot.q.qq.com/wiki/develop/api/openapi/error/error.html#%E9%94%99%E8%AF%AF%E7%A0%81%E5%A4%84%E7%90%86">错误码处理</a> 304023与304024
		/// </summary>
		/// <param name="message">要发送的消息</param>
		/// <param name="channalID">文字子频道ID</param>
		/// <returns>消息</returns>
		/// <exception cref="APIException"></exception>
		public async Task<Message> SendChannelMessageAsync(Models.Guild.MessageReq message, string channalID)
			=> await _botContext.SendChannelMessageAsync(message, channalID);

		/// <summary>
		/// 发送频道私信消息，需要先调用<see cref="CreateDirectMessageSessionAsync(DirectMessageSessionCreateInfo)"/>创建私信会话，再调用此接口发送消息，否则会报错<br/>注意：如果消息需要审核，会抛出异常，详见 <a href="https://bot.q.qq.com/wiki/develop/api/openapi/error/error.html#%E9%94%99%E8%AF%AF%E7%A0%81%E5%A4%84%E7%90%86">错误码处理</a> 304023与304024
		/// </summary>
		/// <param name="message">要发送的消息</param>
		/// <param name="guildID">用户的ID，需要通过<see cref="CreateDirectMessageSessionAsync(DirectMessageSessionCreateInfo)"/>获取</param>
		/// <returns>消息</returns>
		/// <exception cref="APIException"></exception>
		public async Task<Message> SendDirectMessageAsync(Models.Guild.MessageReq message, string guildID)
			=> await _botContext.SendDirectMessageAsync(message, guildID);

		/// <summary>
		/// 创建频道私信会话<br/>用于机器人和在同一个频道内的成员创建私信会话。
		/// </summary>
		/// <param name="info">私信会话创建信息</param>
		/// <returns>私信会话</returns>
		/// <exception cref="APIException"></exception>
		public async Task<DirectMessageSession> CreateDirectMessageSessionAsync(DirectMessageSessionCreateInfo info)
			=> await _botContext.CreateDirectMessageSessionAsync(info);

		/// <summary>
		/// 撤回文字子频道消息
		/// </summary>
		/// <param name="channelID">文字子频道ID</param>
		/// <param name="messageID">消息ID</param>
		/// <param name="hideTip">是否隐藏提示小灰条</param>
		/// <returns></returns>
		/// <exception cref="APIException"></exception>
		public async Task DeleteChannelMessageAsync(string channelID, string messageID, bool hideTip = false)
			=> await _botContext.DeleteChannelMessageAsync(channelID, messageID, hideTip);

		/// <summary>
		/// 撤回文字子频道消息
		/// </summary>
		/// <param name="message">消息</param>
		/// <param name="hideTip">是否隐藏提示小灰条</param>
		/// <returns></returns>
		/// <exception cref="APIException"></exception>
		public async Task DeleteChannelMessageAsync(Message message, bool hideTip = false)
			=> await _botContext.DeleteChannelMessageAsync(message.ChannelID, message.ID, hideTip);

		/// <summary>
		/// 撤回频道私信消息
		/// </summary>
		/// <param name="guildID">用户ID</param>
		/// <param name="messageID">消息ID</param>
		/// <param name="hideTip">是否隐藏提示小灰条</param>
		/// <returns></returns>
		/// <exception cref="APIException"></exception>
		public async Task DeleteDirectMessageAsync(string guildID, string messageID, bool hideTip = false)
			=> await _botContext.DeleteDirectMessageAsync(guildID, messageID, hideTip);

		/// <summary>
		/// 撤回频道私信消息
		/// </summary>
		/// <param name="message">消息</param>
		/// <param name="hideTip">是否隐藏提示小灰条</param>
		/// <returns></returns>
		/// <exception cref="APIException"></exception>
		public async Task DeleteDirectMessageAsync(Message message, bool hideTip = false)
			=> await _botContext.DeleteDirectMessageAsync(message.GuildID, message.ID, hideTip);

		/// <summary>
		/// 发送单聊消息
		/// </summary>
		/// <param name="message">消息</param>
		/// <param name="openID">QQ用户的OpenID，可在各类事件中获得</param>
		/// <returns>发送消息结果</returns>
		/// <exception cref="APIException"></exception>
		public async Task<MessageRes> SendUserMessageAsync(Models.QQ.MessageReq message, string openID)
			=> await _botContext.SendUserMessageAsync(message, openID);

		/// <summary>
		/// 发送群聊消息
		/// </summary>
		/// <param name="message">消息</param>
		/// <param name="groupOpenID">群聊的OpenID</param>
		/// <returns>发送消息结果</returns>
		/// <exception cref="APIException"></exception>
		public async Task<MessageRes> SendGroupMessageAsync(Models.QQ.MessageReq message, string groupOpenID)
			=> await _botContext.SendGroupMessageAsync(message, groupOpenID);

		/// <summary>
		/// 上传用户媒体
		/// </summary>
		/// <param name="media">媒体</param>
		/// <param name="openID">QQ用户的OpenID，可在各类事件中获得</param>
		/// <returns>上传媒体结果</returns>
		/// <exception cref="APIException"></exception>
		public async Task<MediaRes> UploadUserMedia(MediaReq media, string openID)
			=> await _botContext.UploadUserMedia(media, openID);

		/// <summary>
		/// 上传群媒体
		/// </summary>
		/// <param name="media">媒体</param>
		/// <param name="groupOpenID">群聊的OpenID</param>
		/// <returns>上传媒体结果</returns>
		/// <exception cref="APIException"></exception>
		public async Task<MediaRes> UploadGroupMedia(MediaReq media, string groupOpenID)
			=> await _botContext.UploadGroupMedia(media, groupOpenID);

		/// <summary>
		/// 机器人发表表情表态
		/// </summary>
		/// <param name="channelID">子频道ID</param>
		/// <param name="messageID">消息ID</param>
		/// <param name="type">表情类型</param>
		/// <param name="emojiID">表情ID</param>
		/// <returns></returns>
		/// <exception cref="APIException"></exception>
		public async Task SetEmojiReactionAsync(string channelID, string messageID, EmojiType type, string emojiID)
			=> await _botContext.SetEmojiReactionAsync(channelID, messageID, type, emojiID);

		/// <summary>
		/// 机器人发表表情表态
		/// </summary>
		/// <param name="message">消息</param>
		/// <param name="type">表情类型</param>
		/// <param name="emojiID">表情ID</param>
		/// <returns></returns>
		/// <exception cref="APIException"></exception>
		public async Task SetEmojiReactionAsync(Message message, EmojiType type, string emojiID)
			=> await _botContext.SetEmojiReactionAsync(message.ChannelID, message.ID, type, emojiID);

		/// <summary>
		/// 机器人发表表情表态
		/// </summary>
		/// <param name="channelID">子频道ID</param>
		/// <param name="messageID">消息ID</param>
		/// <param name="emoji">表情</param>
		/// <returns></returns>
		/// <exception cref="APIException"></exception>
		public async Task SetEmojiReactionAsync(string channelID, string messageID, Emoji emoji)
			=> await _botContext.SetEmojiReactionAsync(channelID, messageID, emoji.Type, emoji.ID);


		/// <summary>
		/// 机器人发表表情表态
		/// </summary>
		/// <param name="message">消息</param>
		/// <param name="emoji">表情</param>
		/// <returns></returns>
		/// <exception cref="APIException"></exception>
		public async Task SetEmojiReactionAsync(Message message, Emoji emoji)
			=> await _botContext.SetEmojiReactionAsync(message.ChannelID, message.ID, emoji.Type, emoji.ID);

		/// <summary>
		/// 删除机器人发表的表情表态
		/// </summary>
		/// <param name="channelID">子频道ID</param>
		/// <param name="messageID">消息ID</param>
		/// <param name="type">表情类型</param>
		/// <param name="emojiID">表情ID</param>
		/// <returns></returns>
		/// <exception cref="APIException"></exception>
		public async Task DeleteEmojiReactionAsync(string channelID, string messageID, EmojiType type, string emojiID)
			=> await _botContext.DeleteEmojiReactionAsync(channelID, messageID, type, emojiID);

		/// <summary>
		/// 删除机器人发表的表情表态
		/// </summary>
		/// <param name="message">消息</param>
		/// <param name="type">表情类型</param>
		/// <param name="emojiID">表情ID</param>
		/// <returns></returns>
		/// <exception cref="APIException"></exception>
		public async Task DeleteEmojiReactionAsync(Message message, EmojiType type, string emojiID)
			=> await _botContext.DeleteEmojiReactionAsync(message.ChannelID, message.ID, type, emojiID);

		/// <summary>
		/// 删除机器人发表的表情表态
		/// </summary>
		/// <param name="channelID">子频道ID</param>
		/// <param name="messageID">消息ID</param>
		/// <param name="emoji">表情</param>
		/// <returns></returns>
		/// <exception cref="APIException"></exception>
		public async Task DeleteEmojiReactionAsync(string channelID, string messageID, Emoji emoji)
			=> await _botContext.DeleteEmojiReactionAsync(channelID, messageID, emoji.Type, emoji.ID);

		/// <summary>
		/// 删除机器人发表的表情表态
		/// </summary>
		/// <param name="message">消息</param>
		/// <param name="emoji">表情</param>
		/// <returns></returns>
		/// <exception cref="APIException"></exception>
		public async Task DeleteEmojiReactionAsync(Message message, Emoji emoji)
			=> await _botContext.DeleteEmojiReactionAsync(message.ChannelID, message.ID, emoji.Type, emoji.ID);

		/// <summary>
		/// 获取消息表情表态的用户列表
		/// </summary>
		/// <param name="channelID">子频道ID</param>
		/// <param name="messageID">消息ID</param>
		/// <param name="type">表情类型</param>
		/// <param name="emojiID">表情ID</param>
		/// <returns>用户对象列表，参考 User，会返回 id, username, avatar</returns>
		/// <exception cref="APIException"></exception>
		public async Task<List<Models.Guild.User>> GetEmojiReactionAsync(string channelID, string messageID, EmojiType type, string emojiID)
			=> await _botContext.GetEmojiReactionAsync(channelID, messageID, type, emojiID);

		/// <summary>
		/// 获取消息表情表态的用户列表
		/// </summary>
		/// <param name="message">消息</param>
		/// <param name="type">表情类型</param>
		/// <param name="emojiID">表情ID</param>
		/// <returns>用户对象列表，参考 User，会返回 id, username, avatar</returns>
		/// <exception cref="APIException"></exception>
		public async Task<List<Models.Guild.User>> GetEmojiReactionAsync(Message message, EmojiType type, string emojiID)
			=> await _botContext.GetEmojiReactionAsync(message.ChannelID, message.ID, type, emojiID);

		/// <summary>
		/// 获取消息表情表态的用户列表
		/// </summary>
		/// <param name="channelID">子频道ID</param>
		/// <param name="messageID">消息ID</param>
		/// <param name="emoji">表情</param>
		/// <returns>用户对象列表，参考 User，会返回 id, username, avatar</returns>
		/// <exception cref="APIException"></exception>
		public async Task<List<Models.Guild.User>> GetEmojiReactionAsync(string channelID, string messageID, Emoji emoji)
			=> await _botContext.GetEmojiReactionAsync(channelID, messageID, emoji.Type, emoji.ID);

		/// <summary>
		/// 获取消息表情表态的用户列表
		/// </summary>
		/// <param name="message">消息</param>
		/// <param name="emoji">表情</param>
		/// <returns>用户对象列表，参考 User，会返回 id, username, avatar</returns>
		/// <exception cref="APIException"></exception>
		public async Task<List<Models.Guild.User>> GetEmojiReactionAsync(Message message, Emoji emoji)
			=> await _botContext.GetEmojiReactionAsync(message.ChannelID, message.ID, emoji.Type, emoji.ID);

		/// <summary>
		/// 回应交互事件<br/>由于 websocket 推送事件是单向的，开发者收到事件之后，需要进行一次"回应"，告知QQ后台，事件已经收到，否则客户端会一直处于loading状态，直到超时
		/// </summary>
		/// <param name="interactionID">交互ID</param>
		/// <returns></returns>
		public async Task RespondToInteractionAsync(string interactionID)
			=> await _botContext.RespondToInteractionAsync(interactionID);

		#endregion

		/// <inheritdoc/>
		public void Dispose()
		{
			_botContext.Dispose();
		}
	}
}
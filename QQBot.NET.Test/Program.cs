﻿using Newtonsoft.Json.Linq;
using QQBot.NET.Models;
using Serilog;
using System.Text;
using System.Text.RegularExpressions;

namespace QQBot.NET.Test
{
    internal class Program
	{
		private static JObject _jConfig = [];

		static async Task Main(string[] args)
		{
			// 配置日志器
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.WriteTo.Console()
				.CreateLogger();

            // 读取配置文件
            //_jConfig = JObject.Parse(File.ReadAllText("config.json"));
            //var info = new BotCreateInfo()
            //{
            //	AppID = (string?)_jConfig["AppID"],
            //	ClientSecret = (string?)_jConfig["ClientSecret"],
            //	Intents = Intents.ALL,
            //};

            var info = new BotCreateInfo()
            {
                AppID = "102461004",
                ClientSecret = "qcOAwiUH4reRE1pdRF3rfTI7wlaPE4uk",
                Intents = Intents.ALL,
            };

            // 创建机器人服务
            using var bot = new BotService(info);

			// 注册事件
			bot.OnReadyAsync += OnReadyAsync;
			bot.OnAtMessageCreateAsync += OnAtMessageCreateAsync;
			bot.OnDirectMessageCreateAsync += OnDirectMessageCreateAsync;
			bot.OnMessageCreateAsync += OnMessageCreateAsync;
			bot.OnC2CMessageCreateAsync += OnC2CMessageCreateAsync;
			bot.OnGroupAtMessageCreateAsync += OnGroupAtMessageCreateAsync;
			bot.OnMessageReactionAddAsync += OnMessageReactionAddAsync;
			bot.OnMessageReactionRemoveAsync += OnMessageReactionRemoveAsync;
			bot.OnInteractionCreateAsync += OnInteractionCreateAsync;
			bot.OnGuildUpdateAsync += OnGuildUpdateAsync;
			bot.OnChannelCreateAsync += OnChannelCreateAsync;

			// 启动和停止
			await bot.StartAsync();
			Console.ReadLine();
			await bot.StopAsync();
		}

		#region 通用事件

		/// <summary>
		/// READY 事件
		/// </summary>
		private static async Task OnReadyAsync(object sender, ContextEventArgs e)
		{
			Log.Information("Ready");

			var user = await e.Service.GetCurrentUserAsync();
			Log.Information($"当前用户：[{user.ID}]{user.Username}");

			await Task.CompletedTask;
		}

		#endregion

		#region 频道测试

		private static readonly Regex _atTestRegex = new("<@![0-9]+> 测试");
		private static readonly Regex _atPrivateTestRegex = new("<@![0-9]+> 私信测试");
		private static readonly Regex _atDeleteTestRegex = new("<@![0-9]+> 撤回测试");
		private static readonly Regex _atEmojiTestRegex = new("<@![0-9]+> 表情测试");
		private static readonly Regex _atMarkDownTestRegex = new("<@![0-9]+> MarkDown测试");
		private static readonly Regex _atGuildsRegex = new("<@![0-9]+> 频道列表测试");
		private static readonly Regex _atGuildRegex = new("<@![0-9]+> 频道测试");
		private static readonly Regex _atChannelRegex = new("<@![0-9]+> 子频道测试");
		private static readonly Regex _atChannelDetailRegex = new("<@![0-9]+> 子频道详情测试 [0-9]+");
		private static readonly Regex _atCreateChannelRegex = new("<@![0-9]+> 创建子频道测试 (?<Name>[0-9A-Za-z一-龥]+)");
		private static readonly Regex _atModifyChannelRegex = new("<@![0-9]+> 修改子频道测试 (?<ID>[0-9]+) (?<Name>[0-9A-Za-z一-龥]+)");
		private static readonly Regex _atDeleteChannelRegex = new("<@![0-9]+> 删除子频道测试 (?<ID>[0-9]+)");
		private static readonly Regex _atChannelOnlineMemberCountRegex = new("<@![0-9]+> 获取子频道在线成员数测试 (?<ID>[0-9]+)");

		/// <summary>
		/// 文字子频道At消息事件
		/// </summary>
		private static async Task OnAtMessageCreateAsync(object sender, AtMessageEventArgs e)
		{
			// 收到 “@Bot 测试” 消息后，回复 “At测试”
			if (_atTestRegex.IsMatch(e.Message.Content))
			{
				await e.ReplyAsync(new()
				{
					Content = "At测试",
					MessageID = e.Message.ID,
				});
			}

			// 收到 “@Bot 私信测试” 消息后，私信回复 “文字频道的私信测试”
			if (_atPrivateTestRegex.IsMatch(e.Message.Content))
			{
				var dms = await e.CreateDirectMessageSessionAsync(new()
				{
					RecipientID = e.Message.Author.ID,
					SourceGuildID = e.Message.GuildID,
				});
				await e.SendDirectMessageAsync(new()
				{
					Content = "文字频道的私信测试",
					MessageID = e.Message.ID,
				}, dms.GuildID);
			}

			// 收到 “@Bot 撤回测试” 消息后，先发送一个消息，过几秒后撤回
			if (_atDeleteTestRegex.IsMatch(e.Message.Content))
			{
				var delay = 5 * 1000;
				var msg = await e.ReplyAsync(new()
				{
					Content = $"该消息将在{delay / 1000}秒后撤回",
					MessageID = e.Message.ID,
				});
				await Task.Delay(delay);
				await e.DeleteChannelMessageAsync(msg);
			}

			// 收到 “@Bot 表情测试” 消息后，先发送一个消息，进行表情测试
			if (_atEmojiTestRegex.IsMatch(e.Message.Content))
			{
				var delay = 3 * 1000;
				var msg = await e.ReplyAsync(new()
				{
					Content = "表情测试",
					MessageID = e.Message.ID,
				});
				await Task.Delay(delay);
				var emoji = new Emoji()
				{
					ID = "128076",
					Type = EmojiType.Emoji,
				};
				await e.SetEmojiReactionAsync(msg, emoji);
				await Task.Delay(delay);
				var users = await e.GetEmojiReactionAsync(msg, emoji);
				var sb = new StringBuilder();
				sb.Append("表情表态列表：");
				foreach (var user in users)
				{
					sb.Append(user.Username);
					sb.Append(' ');
				}
				await e.ReplyAsync(new()
				{
					Content = sb.ToString(),
					MessageID = e.Message.ID,
				});
				await Task.Delay(delay);
				await e.DeleteEmojiReactionAsync(msg, emoji);
			}

			// 收到 “@Bot MarkDown测试” 消息后，进行MarkDown测试
			if (_atMarkDownTestRegex.IsMatch(e.Message.Content))
			{
				var builder = new MarkDownBuilder();
				builder.At(e.Message.Author.ID);
				builder.Text(" MarkDown测试\n");
				builder.Command("/MarkDown测试");
				await e.ReplyAsync(new()
				{
					Markdown = builder.Build(),
				});
			}

			// 收到 “@Bot 频道列表测试” 消息后，回复频道列表
			// 腾讯你逆大天
			// 我长这么大第一次见GET请求中带Content的
			// 不愧是你
			if (_atGuildsRegex.IsMatch(e.Message.Content))
			{
				// 还有一件事
				// 这API为什么会死循环的啊，已经设置了after，返回的还是一样的内容
				var guilds = await e.GetGuildsAsync();
				var sb = new StringBuilder();
				sb.Append("频道列表：");
				foreach (var guild in guilds)
				{
					sb.Append(guild.Name);
					sb.Append(' ');
				}
				await e.ReplyAsync(new()
				{
					Content = sb.ToString(),
					MessageID = e.Message.ID,
				});
			}

			// 收到 “@Bot 频道测试” 消息后，回复频道信息
			if (_atGuildRegex.IsMatch(e.Message.Content))
			{
				var guild = await e.GetGuildAsync(e.Message.GuildID);
				await e.ReplyAsync(new()
				{
					Content = $"频道ID：{guild.ID}\n频道名称：{guild.Name}\n频道简介：{guild.Description}",
					MessageID = e.Message.ID,
				});
			}

			// 收到 “@Bot 子频道测试” 消息后，回复子频道信息
			if (_atChannelRegex.IsMatch(e.Message.Content))
			{
				var channels = await e.GetChannelsAsync(e.Message.GuildID);
				var sb = new StringBuilder();
				sb.Append("子频道列表：");
				foreach (var channel in channels)
				{
					sb.Append('[');
					sb.Append(channel.ID);
					sb.Append(']');
					sb.Append(channel.Name);
					sb.Append(' ');
				}
				await e.ReplyAsync(new()
				{
					Content = sb.ToString(),
					MessageID = e.Message.ID,
				});
			}

			// 收到 “@Bot 子频道详情测试 ChannelID” 消息后，回复子频道详细信息
			if (_atChannelDetailRegex.IsMatch(e.Message.Content))
			{
				var segments = e.Message.Content.Split(' ');
				var channel = await e.GetChannelAsync(segments[2]);
				await e.ReplyAsync(new()
				{
					Content = $"子频道ID：{channel.ID}\n子频道名称：{channel.Name}\n子频道类型：{channel.Type}",
					MessageID = e.Message.ID,
				});
			}

			// 收到 “@Bot 创建子频道测试” 消息后，创建子频道
			var match = _atCreateChannelRegex.Match(e.Message.Content);
			if (match.Success)
			{
				var channel = await e.CreateChannelAsync(e.Message.GuildID, new()
				{
					Name = match.Groups["Name"].Value,
					Type = ChannelType.Text,
					SubType = ChannelSubType.Chat,
					PrivateType = PrivateType.Public,
					SpeakPermission = SpeakPermission.All,
				});
				await e.ReplyAsync(new()
				{
					Content = $"创建子频道ID：{channel.ID}\n子频道名称：{channel.Name}\n子频道类型：{channel.Type}",
					MessageID = e.Message.ID,
				});
			}

			// 收到 “@Bot 修改子频道测试 <ID> <Name>” 消息后，修改子频道
			match = _atModifyChannelRegex.Match(e.Message.Content);
			if (match.Success)
			{
				var id = match.Groups["ID"].Value;
				var channel = await e.GetChannelAsync(id);
				var name = match.Groups["Name"].Value;
				channel = await e.ModifyChannelAsync(id, new()
				{
					Name = name,
					ParentID = channel.ParentID,
					Position = channel.Position,
					PrivateType = channel.PrivateType,
					SpeakPermission = channel.SpeakPermission,
				});
				await e.ReplyAsync(new()
				{
					Content = $"修改子频道ID：{channel.ID}\n子频道新名称：{channel.Name}",
					MessageID = e.Message.ID,
				});
			}

			// 收到 “@Bot 删除子频道测试 <ID>” 消息后，删除子频道
			match = _atDeleteChannelRegex.Match(e.Message.Content);
			if (match.Success)
			{
				var id = match.Groups["ID"].Value;
				var channel = await e.GetChannelAsync(id);
				await e.DeleteChannelAsync(id);
				await e.ReplyAsync(new()
				{
					Content = $"删除子频道ID：{channel.ID}\n子频道名称：{channel.Name}",
					MessageID = e.Message.ID,
				});
			}

			// 收到 “@Bot 获取子频道在线成员数测试 <ID>” 消息后，回复在线成员数
			match = _atChannelOnlineMemberCountRegex.Match(e.Message.Content);
			if (match.Success)
			{
				var id = match.Groups["ID"].Value;
				var count = await e.GetChannelOnlineMemberCountAsync(id);
				await e.ReplyAsync(new()
				{
					Content = $"子频道在线成员数：{count}",
					MessageID = e.Message.ID,
				});
			}
		}

		/// <summary>
		/// 频道私信事件
		/// </summary>
		private static async Task OnDirectMessageCreateAsync(object sender, DirectMessageEventArgs e)
		{
			// 收到 “测试” 消息后，回复 “私信测试”
			if (e.Message.Content == "测试")
			{
				await e.ReplyAsync(new()
				{
					Content = "私信测试",
					MessageID = e.Message.ID,
				});
			}

			// 收到 “撤回测试” 消息后，先发送一个消息，过几秒后撤回
			if (e.Message.Content == "撤回测试")
			{
				var delay = 5 * 1000;
				var msg = await e.ReplyAsync(new()
				{
					Content = $"该消息将在{delay / 1000}秒后撤回",
					MessageID = e.Message.ID,
				});
				await Task.Delay(delay);
				await e.DeleteDirectMessageAsync(msg);
			}
		}

		/// <summary>
		/// 文字子频道全量消息事件（仅私域）
		/// </summary>
		private static async Task OnMessageCreateAsync(object sender, GuildMessageEventArgs e)
		{
			// 收到 “测试” 消息后，回复 “文字频道测试”
			if (e.Message.Content == "测试")
			{
				await e.ReplyAsync(new()
				{
					Content = "文字频道测试",
					MessageID = e.Message.ID,
				});
			}
		}

		/// <summary>
		/// 消息表态添加事件
		/// </summary>
		private static async Task OnMessageReactionAddAsync(object sender, MessageReactionEventArgs e)
		{
			Log.Information("MessageReactionAdd");

			await Task.CompletedTask;
		}

		/// <summary>
		/// 消息表态移除事件
		/// </summary>
		private static async Task OnMessageReactionRemoveAsync(object sender, MessageReactionEventArgs e)
		{
			Log.Information("MessageReactionRemove");

			await Task.CompletedTask;
		}

		/// <summary>
		/// 按钮交互
		/// </summary>
		private static async Task OnInteractionCreateAsync(object sender, InteractionEventArgs e)
		{
			Log.Information("InteractionCreate");

			// 由于 websocket 推送事件是单向的，开发者收到事件之后，需要进行一次"回应"，告知QQ后台，事件已经收到，否则客户端会一直处于loading状态，直到超时。
			await e.RespondToInteractionAsync(e.Interaction.ID);
		}

		private static async Task OnGuildUpdateAsync(object sender, GuildUpdateEventArgs e)
		{
			Log.Information("GuildUpdate");

			await Task.CompletedTask;
		}

		private static async Task OnChannelCreateAsync(object sender, ChannelCreateEventArgs e)
		{
			Log.Information($"ChannelCreate:[{e.Channel.ID}]{e.Channel.Name}");

			await Task.CompletedTask;
		}

		#endregion

		#region QQ聊天测试

		/// <summary>
		/// 单聊事件
		/// </summary>
		private static async Task OnC2CMessageCreateAsync(object sender, Models.QQ.QQMessageEventArgs e)
		{
			await e.ReplyAsync(new()
			{
				Content = "私聊测试",
				Type = QQMessageType.Text,
				MessageID = e.Message.ID,
			});
		}

		/// <summary>
		/// 群聊事件
		/// </summary>
		private static async Task OnGroupAtMessageCreateAsync(object sender, Models.QQ.QQMessageEventArgs e)
		{
			await e.ReplyAsync(new()
			{
				Content = "群聊测试",
				Type = QQMessageType.Text,
				MessageID = e.Message.ID,
			});
		}

		#endregion
	}
}

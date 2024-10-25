using Newtonsoft.Json;

namespace QQBot.NET.Models
{
	/// <summary>
	/// 子频道在线成员数结果
	/// </summary>
	public class ChannelOnlineMemberCountRes
    {
		/// <summary>
		/// 在线成员数
		/// </summary>
		[JsonProperty("online_nums")]
        public int OnlineNumbers { get; set; }
    }
}

using JetBrains.Annotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.Localization
{
	/// <summary>
	/// Redis 多语言配置
	/// </summary>
	public class LanguageRedisOptions
	{
		/// <summary>
		/// 默认语言
		/// </summary>
		[NotNull]
		[Required]
		public string DefaultLanguage { get; set; } = "zh";

		/// <summary>
		/// Key 前缀，如 language。
		/// <para>框架将会拼接成 language:zh-cn 类似的 key。</para>
		/// </summary>
		[NotNull]
		[Required]
		public string KeyPrefix { get; set; }

		/// <summary>
		/// 缓存的 Key 数量
		/// </summary>
		public int Capacity { get; set; }

		/// <summary>
		/// 每个 Key 一直未被使用，则此时间后过期
		/// </summary>
		public TimeSpan CheckExpired { get; set; } = TimeSpan.FromHours(1);

		/// <summary>
		/// 如果在本地没有搜索到
		/// </summary>
		public TimeSpan CheckNewLanguageExpired { get; set; } = TimeSpan.FromMinutes(1);
	}
}

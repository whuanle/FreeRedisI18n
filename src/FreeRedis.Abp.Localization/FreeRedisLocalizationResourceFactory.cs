using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Localization;

namespace FreeRedis.Abp.Localization
{
	/// <inheritdoc />
	public class FreeRedisLocalizationResourceFactory : ILocalizationResourceContributor
	{
		private readonly RedisClient _redisClient;
		private readonly LanguageRedisOptions _options;

		private volatile bool _isInit = false;

		/// <inheritdoc />
		public FreeRedisLocalizationResourceFactory(RedisClient redisClient, LanguageRedisOptions options)
		{
			_redisClient = redisClient;
			_options = options;

			redisClient.UseClientSideCaching(new ClientSideCachingOptions
			{
				Capacity = _options.Capacity,
				KeyFilter = key => key.StartsWith(_options.KeyPrefix),
				CheckExpired = (key, dt) => DateTime.Now.Subtract(dt) > options.CheckExpired
			});
		}

		/// <inheritdoc />
		public bool IsDynamic => true;

		// 动态语言集，此方法不会被调用
		/// <inheritdoc />
		public void Fill(string cultureName, Dictionary<string, LocalizedString> dictionary)
		{
		}

		// 动态语言集，此方法不会被调用
		/// <inheritdoc />
		public Task FillAsync(string cultureName, Dictionary<string, LocalizedString> dictionary)
		{
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		public LocalizedString GetOrNull(string cultureName, string name)
		{
			if (string.IsNullOrEmpty(cultureName)) cultureName = _options.DefaultLanguage;
			// ex: language:zh-CN
			var key = _options.KeyPrefix + ":" + cultureName;
			var value = _redisClient.HGet(key, name);
			if (string.IsNullOrEmpty(value))
			{
				return new LocalizedString(name, name, resourceNotFound: true);
			}
			return new LocalizedString(name, value);
		}

		/// <inheritdoc />
		public async Task<IEnumerable<string>> GetSupportedCulturesAsync()
		{
			List<string> langs = new List<string>();
			var keys = await _redisClient.KeysAsync(_options.KeyPrefix + "*");
			foreach (var item in keys)
			{
				langs.Add(item.Split(":").LastOrDefault());
			}
			return langs;
		}

		/// <inheritdoc />
		public void Initialize(LocalizationResourceInitializationContext context)
		{
			if (_isInit) return;
			_isInit = true;
			var keys = _redisClient.Keys(_options.KeyPrefix + "*");

			// 第一次先将缓存拉取到本地
			foreach (var key in keys)
			{
				_redisClient.HGetAll(key);
			}
		}
	}
}

using FreeRedis;
using FreeRedis.Abp.Localization;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Volo.Abp.Localization
{
	/// <summary>
	/// 扩展
	/// </summary>
	public static class Extensions
	{
		// 防止每个程序集都缓存
		private static readonly Dictionary<RedisClient, FreeRedisLocalizationResourceFactory> Factories = new Dictionary<RedisClient, FreeRedisLocalizationResourceFactory>();

		/// <summary>
		/// 从 Redis 中添加多语言资源
		/// </summary>
		/// <typeparam name="TLocalizationResource"></typeparam>
		/// <param name="localizationResource"></param>
		/// <param name="redisClient">RedisClient</param>
		/// <param name="options">配置</param>
		/// <returns></returns>
		public static TLocalizationResource AddFreeRedis<TLocalizationResource>(
			[NotNull] this TLocalizationResource localizationResource,
			[NotNull] RedisClient redisClient,
			[NotNull] LanguageRedisOptions options)
			where TLocalizationResource : LocalizationResourceBase
		{
			FreeRedisLocalizationResourceFactory factory;

			if(!Factories.TryGetValue(redisClient,out factory))
			{
				factory = new FreeRedisLocalizationResourceFactory(redisClient, options);
				Factories.Add(redisClient, factory);
			}

			FreeRedisLocalizationResource resource = new FreeRedisLocalizationResource(factory);
			localizationResource.Contributors.Add(resource);
			return localizationResource;
		}
	}
}

using FreeRedis;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace abpapi_net8
{
	[DependsOn(typeof(AbpLocalizationModule))]
	public class ApiModule : AbpModule
	{
		public override void ConfigureServices(ServiceConfigurationContext context)
		{
			// 将 RedisClient 注册为单例
			RedisClient redisClient = new RedisClient("127.0.0.1:6379,defaultDatabase=13");
			LanguageRedisOptions redisOptions = new LanguageRedisOptions
			{
				KeyPrefix = "language",
				Capacity = 20,
				CheckExpired = TimeSpan.FromHours(1),
				CheckNewLanguageExpired = TimeSpan.FromMinutes(1),
			};
			context.Services.AddSingleton<RedisClient>(redisClient);
			context.Services.AddSingleton<LanguageRedisOptions>(redisOptions);


			Configure<AbpVirtualFileSystemOptions>(options =>
			{
				options.FileSets.AddEmbedded<ApiModule>();
			});

			Configure<AbpLocalizationOptions>(options =>
			{
				options.Resources
					.Add<TestResource>("en")
					// 注入 Redis 多语言
					.AddFreeRedis(redisClient, redisOptions)
					.AddVirtualJson("/Localization/Resources/Test");
			});
		}
	}
}

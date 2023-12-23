
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;
using Volo.Abp;
using FreeRedis;
using Volo.Abp.Localization;
using Volo.Abp.VirtualFileSystem;

namespace abpapi_net8_server
{
	[DependsOn(typeof(AbpAspNetCoreMvcModule), typeof(AbpLocalizationModule))]
	public class AppModule : AbpModule
	{
		public override void ConfigureServices(ServiceConfigurationContext context)
		{
			// 将 RedisClient 注册为单例
			RedisClient redisClient = new RedisClient("127.0.0.1:6379,defaultDatabase=13");

			redisClient.HMSet("language:en", new Dictionary<string, string>
			{
				{ "Hello", "Hello" },
				{ "World", "World"},
			});
			redisClient.HMSet("language:zh-CN", new Dictionary<string, string>
			{
				{ "Hello", "你好" },
				{ "World", "世界" },
			});
			redisClient.HMSet("language:zh", new Dictionary<string, string>
			{
				{ "Hello", "你好" },
				{ "World", "世界" },
			});

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
				options.FileSets.AddEmbedded<AppModule>();
			});

			Configure<AbpLocalizationOptions>(options =>
			{
				options.Resources
					.Add<TestResource>("en")
					// 注入 Redis 多语言
					.AddFreeRedis(redisClient, redisOptions);
			});

			context.Services.AddSwaggerGen();
		}
		public override void OnApplicationInitialization(ApplicationInitializationContext context)
		{
			var app = context.GetApplicationBuilder();
			var env = context.GetEnvironment();

			// Configure the HTTP request pipeline.
			if (env.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseAuthorization();

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseRouting();
			app.UseConfiguredEndpoints();
		}
	}
}

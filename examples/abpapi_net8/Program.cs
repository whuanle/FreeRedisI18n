using FreeRedis;
using Microsoft.Extensions.Localization;
using Volo.Abp;
using Volo.Abp.Localization;

namespace abpapi_net8
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			RedisClient redisClient = new RedisClient("127.0.0.1:6379,defaultDatabase=13");

			await redisClient.HMSetAsync("language:en", new Dictionary<string, string>
			{
				{ "Hello", "Hello" },
				{ "World", "World"},
			});
			await redisClient.HMSetAsync("language:zh-CN", new Dictionary<string, string>
			{
				{ "Hello", "你好" },
				{ "World", "世界" },
			});
			await redisClient.HMSetAsync("language:zh", new Dictionary<string, string>
			{
				{ "Hello", "你好" },
				{ "World", "世界" },
			});

			var provider = AbpApplicationFactory.Create<ApiModule>();
			provider.Initialize();
			var localizer = provider.ServiceProvider.GetRequiredService<IStringLocalizer<TestResource>>();
			using (CultureHelper.Use("en"))
			{
				if (localizer["Hello"].Value != "Hello")
				{
					throw new Exception();
				}
			}
			using (CultureHelper.Use("zh"))
			{
				if (localizer["Hello"].Value != "你好")
				{
					throw new Exception();
				}
			}
		}
	}
}

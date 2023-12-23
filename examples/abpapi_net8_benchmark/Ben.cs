using abpapi_net8;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp;
using Volo.Abp.Localization;

namespace abpapi_net8_benchmark
{
	[SimpleJob(RuntimeMoniker.Net70)]
	[SimpleJob(RuntimeMoniker.Net80)]
	//[SimpleJob(RuntimeMoniker.NativeAot70)]
	// GC 分析
	[MemoryDiagnoser]
	// 线程体积
	[ThreadingDiagnoser]
	public partial class Ben
	{
		private IStringLocalizer<TestResource> _stringLocalizer;

		[GlobalSetup]
		public void Setup()
		{
			var application = AbpApplicationFactory.Create<ApiModule>();

			application.Initialize();

			_stringLocalizer = application.ServiceProvider.GetRequiredService<IStringLocalizer<TestResource>>();

			var language = "zh";
			using (CultureHelper.Use(language))
			{
				var value = _stringLocalizer["Hello"];
			}
		}


		[Benchmark]
		public void GetValue()
		{
			using (CultureHelper.Use("zh"))
			{
				var value = _stringLocalizer["Hello"];
			}
		}
	}
}

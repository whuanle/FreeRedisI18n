using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace abpapi_net8_server.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class IndexController : ControllerBase
	{
		private readonly IStringLocalizer<TestResource> _localizer;

		public IndexController(IStringLocalizer<TestResource> localizer)
		{
			_localizer = localizer;
		}

		/// <summary>
		/// »ñÈ¡×Ö·û´®Öµ
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		[HttpGet("get")]
		public string GetAsync(string name)
		{
			return _localizer[name];
		}
	}
}

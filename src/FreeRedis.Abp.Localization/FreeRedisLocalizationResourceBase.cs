using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Localization;

namespace FreeRedis.Abp.Localization
{

	/// <inheritdoc />
	public class FreeRedisLocalizationResource : ILocalizationResourceContributor
	{
		private readonly FreeRedisLocalizationResourceFactory _factory;

		/// <inheritdoc />
		public FreeRedisLocalizationResource(FreeRedisLocalizationResourceFactory factory)
		{
			_factory = factory;
		}

		/// <inheritdoc />
		public bool IsDynamic => true;

		/// <inheritdoc />
		public void Fill(string cultureName, Dictionary<string, LocalizedString> dictionary) => _factory.Fill(cultureName, dictionary);

		/// <inheritdoc />
		public Task FillAsync(string cultureName, Dictionary<string, LocalizedString> dictionary)
		=> _factory.FillAsync(cultureName, dictionary);

		/// <inheritdoc />
		public LocalizedString GetOrNull(string cultureName, string name) => _factory.GetOrNull(cultureName, name);

		/// <inheritdoc />
		public Task<IEnumerable<string>> GetSupportedCulturesAsync()
			=> _factory.GetSupportedCulturesAsync();

		/// <inheritdoc />
		public void Initialize(LocalizationResourceInitializationContext context)
		=> _factory.Initialize(context);
	}
}

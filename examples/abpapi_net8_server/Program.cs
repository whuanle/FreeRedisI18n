namespace abpapi_net8_server
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			await builder.AddApplicationAsync<AppModule>();

			var app = builder.Build();

			await app.InitializeApplicationAsync();
			await app.RunAsync();
		}
	}
}

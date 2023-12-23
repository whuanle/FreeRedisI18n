using BenchmarkDotNet.Running;

namespace abpapi_net8_benchmark
{

	internal class Program
	{
		static void Main(string[] args)
		{
			var summary = BenchmarkRunner.Run<Ben>();
			Console.ReadLine();
		}
	}
}

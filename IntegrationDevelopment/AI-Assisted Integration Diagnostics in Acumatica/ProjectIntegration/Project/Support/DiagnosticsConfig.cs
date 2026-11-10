using System.IO;

namespace IntegrationDiagnostics.Runner
{
	internal static class DiagnosticsConfig
	{
		public const string BaseUrl = "http://localhost:5555";
		public const string Username = "admin";
		public const string Password = "123";
		public const string Company = "";

		public static readonly string OutputDirectory = Path.Combine("outputs", "scenarios");
	}
}

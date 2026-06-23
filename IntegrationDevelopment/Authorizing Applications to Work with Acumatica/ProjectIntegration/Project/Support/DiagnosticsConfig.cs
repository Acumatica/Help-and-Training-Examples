using System.IO;

namespace IntegrationDiagnostics.Runner
{
	internal static class DiagnosticsConfig
	{
		public const string BaseUrl = "https://bel-u23-nodeb:30050/";
		public const string Username = "admin";
		public const string Password = "123";
		public const string Company = "Company";

		public static readonly string OutputDirectory = Path.Combine("outputs", "scenarios");
	}
}

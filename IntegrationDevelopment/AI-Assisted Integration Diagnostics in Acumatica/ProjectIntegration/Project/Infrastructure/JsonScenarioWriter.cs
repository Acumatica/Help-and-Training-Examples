using System;
using System.IO;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace IntegrationDiagnostics.Runner
{
	internal sealed class JsonScenarioWriter
	{
		private readonly JsonSerializerSettings _settings = new JsonSerializerSettings
		{
			Formatting = Formatting.Indented,
			NullValueHandling = NullValueHandling.Ignore,
			ReferenceLoopHandling = ReferenceLoopHandling.Ignore
		};

		public string OutputDirectory { get; private set; }

		public JsonScenarioWriter(string outputDirectory)
		{
			OutputDirectory = Path.GetFullPath(outputDirectory);
			Directory.CreateDirectory(OutputDirectory);
		}

		public void Write(ScenarioContext context)
		{
			var path = Path.Combine(OutputDirectory, SafeFileName(context.ScenarioName) + ".json");
			File.WriteAllText(path, JsonConvert.SerializeObject(context.ToJsonModel(), _settings), Encoding.UTF8);
		}

		private static string SafeFileName(string value)
		{
			var invalid = Path.GetInvalidFileNameChars();
			var cleaned = new string(value.Select(character => invalid.Contains(character) ? '_' : character).ToArray());
			return String.IsNullOrWhiteSpace(cleaned) ? "scenario" : cleaned;
		}
	}
}

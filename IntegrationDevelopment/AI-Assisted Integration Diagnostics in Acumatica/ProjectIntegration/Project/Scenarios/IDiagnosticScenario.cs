using Acumatica.RESTClient.Client;

namespace IntegrationDiagnostics.Runner
{
	internal interface IDiagnosticScenario
	{
		string Name { get; }

		void Run(ApiClient client, ScenarioContext context);
	}
}

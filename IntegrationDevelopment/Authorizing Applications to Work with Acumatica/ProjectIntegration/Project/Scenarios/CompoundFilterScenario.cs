using Acumatica.Default_25_200_001.Model;
using Acumatica.RESTClient.Client;
using Acumatica.RESTClient.ContractBasedApi;

namespace IntegrationDiagnostics.Runner
{
	internal sealed class CompoundFilterScenario : IDiagnosticScenario
	{
		public string Name
		{
			get { return "compound-filter"; }
		}

		public void Run(ApiClient client, ScenarioContext context)
		{
			context.Step(
				"GET events with compound filter",
				() =>
				{
					const string filter =
						"Status eq 'Open'" +
						" and EndDateTime gt datetimeoffset'2024-01-01T00:00:00Z'";

					return new
					{
						Filter = filter,
						Records = client.GetList<Event>(filter: filter, top: 20)
					};
				});
		}
	}
}

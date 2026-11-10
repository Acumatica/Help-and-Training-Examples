using Acumatica.Default_22_200_001.Model;
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
						" and EndDate gt date'2024-01-01'";

					return new
					{
						Filter = filter,
						Records = client.GetList<Event>(filter: filter, top: 20)
					};
				});
		}
	}
}
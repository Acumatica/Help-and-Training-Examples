using Acumatica.Default_22_200_001.Model;
using Acumatica.RESTClient.Client;
using Acumatica.RESTClient.ContractBasedApi;
using Acumatica.RESTClient.ContractBasedApi.Model;

namespace IntegrationDiagnostics.Runner
{
	internal sealed class CostCodeScenario : IDiagnosticScenario
	{
		public string Name
		{
			get { return "cost-codes"; }
		}

		public void Run(ApiClient client, ScenarioContext context)
		{
			var costCodeId = IdGenerator.CostCodeId();

			var costCode = context.Step(
				"PUT create",
				() => client.Put(new CostCode
				{
					CostCodeID = new StringValue { Value = costCodeId },
					Description = new StringValue { Value = "Diagnostic cost code " + costCodeId }
				}));

			costCode = context.Step(
				"GET by keys",
				() => client.GetByKeys<CostCode>(costCodeId));

			costCode.Description = new StringValue { Value = "Updated diagnostic cost code " + costCodeId };

			context.Step(
				"PUT update",
				() => client.Put(costCode));

			context.Step(
				"GET list",
				() => client.GetList<CostCode>(filter: "CostCodeID eq '" + costCodeId + "'"));
		}
	}
}

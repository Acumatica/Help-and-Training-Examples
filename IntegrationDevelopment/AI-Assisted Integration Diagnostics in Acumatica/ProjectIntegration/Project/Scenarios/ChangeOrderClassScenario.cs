using Acumatica.Default_22_200_001.Model;
using Acumatica.RESTClient.Client;
using Acumatica.RESTClient.ContractBasedApi;
using Acumatica.RESTClient.ContractBasedApi.Model;

namespace IntegrationDiagnostics.Runner
{
	internal sealed class ChangeOrderClassScenario : IDiagnosticScenario
	{
		public string Name
		{
			get { return "change-order-classes"; }
		}

		public void Run(ApiClient client, ScenarioContext context)
		{
			var classId = IdGenerator.ChangeOrderClassId();

			var changeOrderClass = context.Step(
				"PUT create change order class",
				() => client.Put(new ChangeOrderClass
				{
					ClassID = new StringValue { Value = classId },
					Description = new StringValue { Value = "Diagnostic change order class " + classId },
					Active = new BooleanValue { Value = false },
					CostBudget = new BooleanValue { Value = false }
				}));

			changeOrderClass = context.Step(
				"GET change order class",
				() => client.GetByKeys<ChangeOrderClass>(classId, expand: "Attributes"));

			changeOrderClass.Description = new StringValue { Value = "Updated diagnostic change order class " + classId };
			changeOrderClass.Active = new BooleanValue { Value = true };

			context.Step(
				"PUT update change order class",
				() => client.Put(changeOrderClass, expand: "Attributes"));
		}
	}
}

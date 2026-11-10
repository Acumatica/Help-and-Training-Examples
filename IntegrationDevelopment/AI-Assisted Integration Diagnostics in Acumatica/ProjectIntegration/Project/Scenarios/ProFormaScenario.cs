using System.Linq;

using Acumatica.Default_22_200_001.Model;
using Acumatica.RESTClient.Client;
using Acumatica.RESTClient.ContractBasedApi;
using Acumatica.RESTClient.ContractBasedApi.Model;

namespace IntegrationDiagnostics.Runner
{
	internal sealed class ProFormaScenario : IDiagnosticScenario
	{
		public string Name
		{
			get { return "pro-forma"; }
		}

		public void Run(ApiClient client, ScenarioContext context)
		{
			var projectId = IdGenerator.ProjectId();
			var taskId = IdGenerator.TaskId();

			context.Step(
				"CREATE billable project transaction and run project billing",
				() => ScenarioHelpers.CreateProForma(client, projectId, taskId, 10, 100));

			var proForma = context.Step(
				"GET pro forma invoice by project",
				() => client.GetList<ProFormaInvoice>(filter: "ProjectID eq '" + projectId + "'").First());

			proForma = context.Step(
				"GET pro forma invoice with time and material",
				() => client.GetById<ProFormaInvoice>(proForma.ID, expand: "TimeAndMaterial"));

			proForma.Description = new StringValue { Value = "Updated diagnostic pro forma" };
			proForma.ExternalRefNbr = new StringValue { Value = "DIAG-PROFORMA" };

			if (proForma.TimeAndMaterial != null && proForma.TimeAndMaterial.Count > 0)
			{
				proForma.TimeAndMaterial[0].Description = new StringValue { Value = "Updated time and material line" };
				proForma.TimeAndMaterial[0].QtyToInvoice = new DecimalValue { Value = 5 };
				proForma.TimeAndMaterial[0].UnitPrice = new DecimalValue { Value = 3 };
				proForma.TimeAndMaterial[0].TaxCategory = new StringValue { Value = "EXEMPT" };
				proForma.TimeAndMaterial[0].SalesAccount = new StringValue { Value = "40000" };
			}

			context.Step(
				"PUT update pro forma invoice",
				() => client.Put(proForma, expand: "TimeAndMaterial"));

			context.Step(
				"RUN remove pro forma from hold action",
				() =>
				{
					ScenarioHelpers.DoAction(client, new RemoveProFormaInvoiceFromHold(proForma));
					return client.GetById<ProFormaInvoice>(proForma.ID);
				});
		}
	}
}

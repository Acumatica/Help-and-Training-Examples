using System.Collections.Generic;

using Acumatica.Default_25_200_001.Model;
using Acumatica.RESTClient.Client;
using Acumatica.RESTClient.ContractBasedApi;
using Acumatica.RESTClient.ContractBasedApi.Model;

namespace IntegrationDiagnostics.Runner
{
	internal sealed class ProjectTransactionScenario : IDiagnosticScenario
	{
		public string Name
		{
			get { return "project-transactions"; }
		}

		public void Run(ApiClient client, ScenarioContext context)
		{
			var transaction = context.Step(
				"PUT create project transaction",
				() => client.Put(CreateTransaction(2, 100), expand: "Details"));

			transaction.Description = new StringValue { Value = "Updated diagnostic transaction" };
			transaction.Details[0].ExternalRefNbr = new StringValue { Value = "DIAG-ERN" };

			transaction = context.Step(
				"PUT update transaction",
				() => client.Put(transaction, expand: "Details"));

			context.Step(
				"GET transaction list",
				() => client.GetList<ProjectTransaction>(
					filter: "ReferenceNbr eq '" + transaction.ReferenceNbr.Value + "'",
					expand: "Details"));

			context.Step(
				"RUN release transactions action",
				() =>
				{
					ScenarioHelpers.DoAction(client, new ReleaseTransactions(transaction));
					return client.GetById<ProjectTransaction>(transaction.ID, expand: "Details");
				});
		}

		private static ProjectTransaction CreateTransaction(decimal qty, decimal unitRate)
		{
			return new ProjectTransaction
			{
				Module = new StringValue { Value = "PM" },
				Description = new StringValue { Value = "Diagnostic project transaction" },
				Details = new List<ProjectTransactionDetail>
				{
					new ProjectTransactionDetail
					{
						Project = new StringValue { Value = "X" },
						Qty = new DecimalValue { Value = qty },
						CostCode = new StringValue { Value = ScenarioHelpers.CostCode },
						UnitRate = new DecimalValue { Value = unitRate }
					},
					new ProjectTransactionDetail
					{
						Project = new StringValue { Value = "X" },
						Qty = new DecimalValue { Value = qty },
						CostCode = new StringValue { Value = ScenarioHelpers.CostCode },
						UnitRate = new DecimalValue { Value = unitRate }
					}
				}
			};
		}
	}
}

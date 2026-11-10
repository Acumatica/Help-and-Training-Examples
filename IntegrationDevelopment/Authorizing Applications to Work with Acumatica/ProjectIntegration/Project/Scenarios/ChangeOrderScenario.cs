using System;
using System.Collections.Generic;

using Acumatica.Default_25_200_001.Model;
using Acumatica.RESTClient.Client;
using Acumatica.RESTClient.ContractBasedApi;
using Acumatica.RESTClient.ContractBasedApi.Model;

namespace IntegrationDiagnostics.Runner
{
	internal sealed class ChangeOrderScenario : IDiagnosticScenario
	{
		private const string Expand = "Commitments,CostBudget,RevenueBudget";

		public string Name
		{
			get { return "change-orders"; }
		}

		public void Run(ApiClient client, ScenarioContext context)
		{
			var projectId = IdGenerator.ProjectId();
			var taskId = IdGenerator.TaskId();

			context.Step(
				"PUT create parent project",
				() => ScenarioHelpers.CreateProject(client, projectId, "Diagnostic change order project"));

			context.Step(
				"PUT create parent task",
				() => ScenarioHelpers.CreateProjectTask(client, projectId, taskId, "Diagnostic change order task"));

			var changeOrder = context.Step(
				"PUT create change order",
				() => client.Put(new ChangeOrder
				{
					ChangeDate = new DateTimeValue { Value = DateTime.Today },
					CompletionDate = new DateTimeValue { Value = DateTime.Today.AddDays(1) },
					Description = new StringValue { Value = "Diagnostic change order" },
					ProjectID = new StringValue { Value = projectId },
					Class = new StringValue { Value = "DEFAULT" },
					DetailedDescription = new StringValue { Value = "Detailed diagnostic change order" }
				}));

			changeOrder = context.Step(
				"GET change order with detail tabs",
				() => client.GetByKeys<ChangeOrder>(changeOrder.RefNbr.Value, expand: Expand));

			changeOrder.Description = new StringValue { Value = "Updated diagnostic change order" };
			changeOrder.RevenueBudget = new List<ChangeOrderRevenueBudget>
			{
				new ChangeOrderRevenueBudget
				{
					ProjectTaskID = new StringValue { Value = taskId },
					AccountGroup = new StringValue { Value = "REVENUE" },
					Description = new StringValue { Value = "Diagnostic revenue budget" },
					Qty = new DecimalValue { Value = 10 }
				}
			};
			changeOrder.CostBudget = new List<ChangeOrderCostBudget>
			{
				new ChangeOrderCostBudget
				{
					ProjectTaskID = new StringValue { Value = taskId },
					AccountGroup = new StringValue { Value = "LABOR" },
					Description = new StringValue { Value = "Diagnostic cost budget" },
					Qty = new DecimalValue { Value = 10 }
				}
			};
			changeOrder.Commitments = new List<ChangeOrderCommitment>
			{
				new ChangeOrderCommitment
				{
					ProjectTaskID = new StringValue { Value = taskId },
					CostCode = new StringValue { Value = ScenarioHelpers.CostCode },
					Description = new StringValue { Value = "Diagnostic commitment" },
					Vendor = new StringValue { Value = "ALLFRUITS" }
				}
			};

			changeOrder = context.Step(
				"PUT update change order details",
				() => client.Put(changeOrder, expand: Expand));

			context.Step(
				"RUN remove change order from hold action",
				() =>
				{
					ScenarioHelpers.DoAction(client, new RemoveChangeOrderFromHold(new ChangeOrder { RefNbr = changeOrder.RefNbr }));
					return client.GetByKeys<ChangeOrder>(changeOrder.RefNbr.Value, expand: Expand);
				});
		}
	}
}

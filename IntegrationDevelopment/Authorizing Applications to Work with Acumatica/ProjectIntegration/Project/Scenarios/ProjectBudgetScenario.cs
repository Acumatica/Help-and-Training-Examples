using Acumatica.Default_25_200_001.Model;
using Acumatica.RESTClient.Client;
using Acumatica.RESTClient.ContractBasedApi;
using Acumatica.RESTClient.ContractBasedApi.Model;

namespace IntegrationDiagnostics.Runner
{
	internal sealed class ProjectBudgetScenario : IDiagnosticScenario
	{
		private const string InventoryIdNa = "<N/A>";
		private const string AccountGroupExpense = "MISC";

		public string Name
		{
			get { return "project-budgets"; }
		}

		public void Run(ApiClient client, ScenarioContext context)
		{
			var projectId = IdGenerator.ProjectId();
			var taskId = IdGenerator.TaskId();

			context.Step(
				"PUT create parent project",
				() => ScenarioHelpers.CreateProject(client, projectId, "Diagnostic budget project"));

			context.Step(
				"PUT create parent task",
				() => ScenarioHelpers.CreateProjectTask(client, projectId, taskId, "Diagnostic budget task"));

			var budget = context.Step(
				"PUT create budget",
				() => client.Put(new ProjectBudget
				{
					ProjectID = new StringValue { Value = projectId },
					ProjectTaskID = new StringValue { Value = taskId },
					AccountGroup = new StringValue { Value = AccountGroupExpense },
					Description = new StringValue { Value = "Diagnostic budget" },
					UnitRate = new DecimalValue { Value = 1 },
					OriginalBudgetedQty = new DecimalValue { Value = 2 },
					OriginalBudgetedAmount = new DecimalValue { Value = 3 },
					InventoryID = new StringValue { Value = InventoryIdNa }
				}));

			budget.Description = new StringValue { Value = "Updated diagnostic budget" };
			budget.OriginalBudgetedAmount = new DecimalValue { Value = 30 };

			context.Step(
				"PUT update budget",
				() => client.Put(budget));

			context.Step(
				"GET project budget list",
				() => client.GetList<ProjectBudget>(filter: "ProjectID eq '" + projectId + "'", expand: ""));
		}
	}
}

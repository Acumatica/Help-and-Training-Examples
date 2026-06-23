using Acumatica.Default_25_200_001.Model;
using Acumatica.RESTClient.Client;
using Acumatica.RESTClient.ContractBasedApi;
using Acumatica.RESTClient.ContractBasedApi.Model;

namespace IntegrationDiagnostics.Runner
{
	internal sealed class ProjectTemplateTaskScenario : IDiagnosticScenario
	{
		private const string Expand = "Properties,BillingAndAllocationSettings,DefaultValues,VisibilitySettings";

		public string Name
		{
			get { return "project-template-tasks"; }
		}

		public void Run(ApiClient client, ScenarioContext context)
		{
			var templateId = IdGenerator.TemplateId();
			var taskId = IdGenerator.TemplateTaskId();

			context.Step(
				"PUT create parent template",
				() => client.Put(new ProjectTemplate
				{
					ProjectTemplateID = new StringValue { Value = templateId },
					Description = new StringValue { Value = "Diagnostic template for task " + templateId },
					GLAccounts = new ProjectGLAccount
					{
						DefaultSubaccount = new StringValue { Value = "100000000000A" }
					}
				}));

			var task = context.Step(
				"PUT create template task",
				() => client.Put(new ProjectTemplateTask
				{
					ProjectTemplateID = new StringValue { Value = templateId },
					ProjectTemplateTaskID = new StringValue { Value = taskId },
					Description = new StringValue { Value = "Diagnostic template task " + taskId },
					Properties = new ProjectTemplateTaskProperties
					{
						Default = new BooleanValue { Value = true },
						CompletionMethod = new StringValue { Value = "Budgeted Quantity" }
					},
					BillingAndAllocationSettings = new ProjectTaskBillingAndAllocationSettings
					{
						AllocationRule = new StringValue { Value = "WIPTM" },
						WIPAccountGroup = new StringValue { Value = "LABOR" }
					},
					DefaultValues = new ProjectTaskDefaultValues
					{
						DefaultAccount = new StringValue { Value = "40000" },
						DefaultCostAccount = new StringValue { Value = ScenarioHelpers.DefaultCostAccount },
						DefaultCostSubaccount = new StringValue { Value = ScenarioHelpers.DefaultCostSubaccount },
						TaxCategory = new StringValue { Value = "EXEMPT" }
					},
					VisibilitySettings = new VisibilitySettings
					{
						GL = new BooleanValue { Value = false },
						SO = new BooleanValue { Value = false },
						Expenses = new BooleanValue { Value = false }
					}
				}, expand: Expand));

			task = context.Step(
				"GET template task",
				() => client.GetList<ProjectTemplateTask>(
					filter: "ProjectTemplateID eq '" + templateId + "' and ProjectTemplateTaskID eq '" + taskId + "'",
					expand: Expand)[0]);

			task.Description = new StringValue { Value = "Updated diagnostic template task " + taskId };
			task.Properties.CompletionMethod = new StringValue { Value = "Budgeted Amount" };
			task.BillingAndAllocationSettings.RateTable = new StringValue { Value = "STANDARD" };
			task.DefaultValues.DefaultCostAccount = new StringValue { Value = "51000" };

			context.Step(
				"PUT update template task",
				() => client.Put(task, expand: Expand));
		}
	}
}

using System;

using Acumatica.Default_25_200_001.Model;
using Acumatica.RESTClient.Client;
using Acumatica.RESTClient.ContractBasedApi;
using Acumatica.RESTClient.ContractBasedApi.Model;

namespace IntegrationDiagnostics.Runner
{
	internal sealed class ProjectLifecycleScenario : IDiagnosticScenario
	{
		private const string Expand = "ProjectProperties,GLAccounts,Employees,Balances,BillingAndAllocationSettings";

		public string Name
		{
			get { return "projects"; }
		}

		public void Run(ApiClient client, ScenarioContext context)
		{
			var projectId = IdGenerator.ProjectId();

			var project = context.Step(
				"PUT create",
				() => client.Put(CreateProject(projectId, "Diagnostic project " + projectId), expand: Expand));

			project = context.Step(
				"GET by keys",
				() => client.GetByKeys<Project>(projectId, expand: Expand));

			project.Description = new StringValue { Value = "Updated diagnostic project " + projectId };
			project.ExternalRefNbr = new StringValue { Value = "DIAG-" + projectId };

			context.Step(
				"PUT update",
				() => client.Put(project, expand: Expand));

			context.Step(
				"GET list",
				() => client.GetList<Project>(filter: "ProjectID eq '" + projectId + "'"));
		}

		private static Project CreateProject(string projectId, string description)
		{
			var startDate = DateTime.Today;
			return new Project
			{
				ProjectID = new StringValue { Value = projectId },
				Description = new StringValue { Value = description },
				Hold = new BooleanValue { Value = false },
				ProjectProperties = new ProjectProperties
				{
					StartDate = new DateTimeValue
					{
						Value = new DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0, DateTimeKind.Unspecified)
					}
				},
				GLAccounts = new ProjectGLAccount
				{
					DefaultSubaccount = new StringValue { Value = ScenarioHelpers.DefaultSubaccount },
					DefaultCostAccount = new StringValue { Value = ScenarioHelpers.DefaultCostAccount },
					DefaultCostSubaccount = new StringValue { Value = ScenarioHelpers.DefaultCostSubaccount }
				}
			};
		}
	}
}

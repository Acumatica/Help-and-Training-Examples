using System.Collections.Generic;

using Acumatica.Default_25_200_001.Model;
using Acumatica.RESTClient.Client;
using Acumatica.RESTClient.ContractBasedApi;
using Acumatica.RESTClient.ContractBasedApi.Model;

namespace IntegrationDiagnostics.Runner
{
	internal sealed class ProjectTemplateScenario : IDiagnosticScenario
	{
		private const string Expand = "ProjectProperties, BillingAndAllocationSettings, VisibilitySettings, Employees, GLAccounts";

		public string Name
		{
			get { return "project-templates"; }
		}

		public void Run(ApiClient client, ScenarioContext context)
		{
			var templateId = IdGenerator.TemplateId();
			var projectId = IdGenerator.ProjectId();

			var template = context.Step(
				"PUT create project template",
				() => client.Put(new ProjectTemplate
				{
					ProjectTemplateID = new StringValue { Value = templateId },
					Description = new StringValue { Value = "Diagnostic project template " + templateId },
					ProjectProperties = new ProjectProperties
					{
						RevenueBudgetLevel = new StringValue { Value = "Task and Cost Code" },
						TrackProductionData = new BooleanValue { Value = true }
					},
					BillingAndAllocationSettings = new ProjectBillingAndAllocationSettings
					{
						BillingPeriod = new StringValue { Value = "Quarter" },
						BillingRule = new StringValue { Value = "PROGRESS" }
					},
					VisibilitySettings = new VisibilitySettings
					{
						AR = new BooleanValue { Value = false },
						PO = new BooleanValue { Value = false }
					},
					Employees = new List<ProjectEmployee>
					{
						new ProjectEmployee { EmployeeID = new StringValue { Value = "EP00000001" } }
					},
					GLAccounts = new ProjectGLAccount
					{
						DefaultAccount = new StringValue { Value = "40000" },
						DefaultSubaccount = new StringValue { Value = "100000000000A" },
						DefaultCostAccount = new StringValue { Value = ScenarioHelpers.DefaultCostAccount },
						DefaultCostSubaccount = new StringValue { Value = ScenarioHelpers.DefaultCostSubaccount }
					}
				}, expand: Expand));

			template = context.Step(
				"GET project template",
				() => client.GetByKeys<ProjectTemplate>(templateId, expand: Expand));

			template.Description = new StringValue { Value = "Updated diagnostic project template " + templateId };
			template.ProjectProperties.RestrictEmployees = new BooleanValue { Value = true };
			template.VisibilitySettings.AR = new BooleanValue { Value = true };

			context.Step(
				"PUT update project template",
				() => client.Put(template, expand: Expand));

			context.Step(
				"RUN activate project template action",
				() =>
				{
					ScenarioHelpers.DoAction(client, new ActivateProjectTemplate(template));
					return client.GetById<ProjectTemplate>(template.ID, expand: Expand);
				});

			context.Step(
				"PUT create project from template",
				() => ScenarioHelpers.CreateProjectFromTemplate(client, projectId, templateId));

			context.Step(
				"PUT update project retainage",
				() => client.Put(new Project
				{
					ProjectID = new StringValue { Value = projectId },
					Retainage = new ProjectRetainage
					{
						RetainagePct = new DecimalValue { Value = 10 }
					}
				}, expand: "Retainage"));
		}
	}
}

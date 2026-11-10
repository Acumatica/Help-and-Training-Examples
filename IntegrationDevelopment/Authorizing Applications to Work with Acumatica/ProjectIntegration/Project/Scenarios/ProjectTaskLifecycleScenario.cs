using System;

using Acumatica.Default_25_200_001.Model;
using Acumatica.RESTClient.Client;
using Acumatica.RESTClient.ContractBasedApi;
using Acumatica.RESTClient.ContractBasedApi.Model;

namespace IntegrationDiagnostics.Runner
{
	internal sealed class ProjectTaskLifecycleScenario : IDiagnosticScenario
	{
		private const string TaskExpand = "DefaultValues";

		public string Name
		{
			get { return "project-tasks"; }
		}

		public void Run(ApiClient client, ScenarioContext context)
		{
			var projectId = IdGenerator.ProjectId();
			var taskId = IdGenerator.TaskId();

			context.Step(
				"PUT create parent project",
				() => client.Put(CreateProject(projectId, "Diagnostic task parent " + projectId)));

			var task = context.Step(
				"PUT create task",
				() => client.Put(new ProjectTask
				{
					ProjectID = new StringValue { Value = projectId },
					ProjectTaskID = new StringValue { Value = taskId },
					Description = new StringValue { Value = "Diagnostic task " + taskId },
					DefaultValues = new ProjectTaskDefaultValues
					{
						DefaultCostAccount = new StringValue { Value = ScenarioHelpers.DefaultCostAccount },
						DefaultCostSubaccount = new StringValue { Value = ScenarioHelpers.DefaultCostSubaccount }
					}
				}, expand: TaskExpand));

			context.Step(
				"GET list",
				() => client.GetList<ProjectTask>(filter: "ProjectID eq '" + projectId + "' and ProjectTaskID eq '" + taskId + "'"));

			task.Description = new StringValue { Value = "Updated diagnostic task " + taskId };
			if (task.DefaultValues == null)
			{
				task.DefaultValues = new ProjectTaskDefaultValues();
			}

			task.DefaultValues.DefaultCostAccount = new StringValue { Value = "51150" };

			context.Step(
				"PUT update",
				() => client.Put(task, expand: TaskExpand));
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
					DefaultSubaccount = new StringValue { Value = ScenarioHelpers.DefaultSubaccount }
				}
			};
		}
	}
}

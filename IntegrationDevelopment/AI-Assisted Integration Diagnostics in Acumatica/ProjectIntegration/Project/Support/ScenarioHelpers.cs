using System;
using System.Collections.Generic;
using System.Linq;

using Acumatica.Default_22_200_001.Model;
using Acumatica.RESTClient.Client;
using Acumatica.RESTClient.ContractBasedApi;
using Acumatica.RESTClient.ContractBasedApi.Model;

namespace IntegrationDiagnostics.Runner
{
	internal static class ScenarioHelpers
	{
		public const string DefaultSubaccount = "0000000000000";
		public const string DefaultCostAccount = "51000";
		public const string DefaultCostSubaccount = "1000000000000";
		public const string CostCode = "0000";
		public const string Customer = "C000000056";
		public const string BillingRule = "TM";
		public const string AccountGroup = "LABOR";

		public static Project CreateProject(ApiClient client, string projectId, string description, string customer = null)
		{
			var startDate = DateTime.Today;
			return client.Put(new Project
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
					DefaultSubaccount = new StringValue { Value = DefaultSubaccount },
					DefaultCostAccount = new StringValue { Value = DefaultCostAccount },
					DefaultCostSubaccount = new StringValue { Value = DefaultCostSubaccount }
				},
				Customer = customer == null ? null : new StringValue { Value = customer }
			});
		}

		public static Project CreateProjectFromTemplate(ApiClient client, string projectId, string templateId)
		{
			return client.Put(new Project
			{
				ProjectID = new StringValue { Value = projectId },
				ProjectTemplateID = new StringValue { Value = templateId }
			});
		}

		public static ProjectTask CreateProjectTask(ApiClient client, string projectId, string taskId, string description)
		{
			var task = client.Put(new ProjectTask
			{
				ProjectID = new StringValue { Value = projectId },
				ProjectTaskID = new StringValue { Value = taskId },
				Description = new StringValue { Value = description }
			});

			DoAction(client, new ActivateProjectTask(task));
			return client.GetById<ProjectTask>(task.ID);
		}

		public static ProjectTransaction CreateProForma(
			ApiClient client,
			string projectId,
			string taskId,
			decimal qty,
			decimal amount)
		{
			var project = CreateProject(client, projectId, "Diagnostic pro forma project", Customer);
			project.Hold = new BooleanValue { Value = false };
			project.Status = new StringValue { Value = "Active" };
			project.GLAccounts = new ProjectGLAccount
			{
				DefaultAccount = new StringValue { Value = "40000" }
			};
			project.BillingAndAllocationSettings = new ProjectBillingAndAllocationSettings
			{
				NextBillingDate = new DateTimeValue { Value = DateTime.Now },
				BillingRule = new StringValue { Value = BillingRule }
			};
			client.Put(project);

			var task = CreateProjectTask(client, projectId, taskId, "Diagnostic pro forma task");
			task.Status = new StringValue { Value = "Active" };
			task.BillingAndAllocationSettings = new ProjectTaskBillingAndAllocationSettings
			{
				BillingRule = new StringValue { Value = BillingRule }
			};
			client.Put(task);

			var transaction = client.Put(new ProjectTransaction
			{
				Details = new List<ProjectTransactionDetail>
				{
					new ProjectTransactionDetail
					{
						Project = new StringValue { Value = projectId },
						ProjectTask = new StringValue { Value = taskId },
						CostCode = new StringValue { Value = CostCode },
						AccountGroup = new StringValue { Value = AccountGroup },
						Qty = new DecimalValue { Value = qty },
						Amount = new DecimalValue { Value = amount }
					}
				}
			});

			var transactionToRelease = client
				.GetList<ProjectTransaction>(filter: "ReferenceNbr eq '" + transaction.ReferenceNbr.Value + "'")
				.First();

			DoAction(client, new ReleaseTransactions(transactionToRelease));

			var projectToBill = client.GetList<Project>(filter: "ProjectID eq '" + projectId + "'").First();
			DoAction(client, new RunProjectBilling(projectToBill));

			return transaction;
		}

		public static void DoAction<T>(ApiClient client, EntityAction<T> action, DateTime? businessDate = null)
			where T : Entity, ITopLevelEntity, new()
		{
			ApiClientExtensions.WaitActionCompletion(
				client,
				ApiClientExtensions.InvokeAction(client, action, businessDate: businessDate));
		}
	}
}

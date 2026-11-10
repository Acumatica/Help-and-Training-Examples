using Acumatica.Default_22_200_001.Model;
using Acumatica.RESTClient.Client;
using Acumatica.RESTClient.ContractBasedApi;
using Acumatica.RESTClient.ContractBasedApi.Model;

namespace IntegrationDiagnostics.Runner
{
	internal sealed class AccountGroupScenario : IDiagnosticScenario
	{
		public string Name
		{
			get { return "account-groups"; }
		}

		public void Run(ApiClient client, ScenarioContext context)
		{
			var accountGroupId = IdGenerator.AccountGroupId();

			var accountGroup = context.Step(
				"PUT create account group",
				() => client.Put(new AccountGroup
				{
					AccountGroupID = new StringValue { Value = accountGroupId },
					Description = new StringValue { Value = "Diagnostic account group " + accountGroupId }
				}));

			accountGroup = context.Step(
				"GET account group",
				() => client.GetByKeys<AccountGroup>(accountGroupId, expand: "Attributes"));

			accountGroup.Description = new StringValue { Value = "Updated diagnostic account group " + accountGroupId };

			context.Step(
				"PUT update account group",
				() => client.Put(accountGroup));

			context.Step(
				"GET account group list",
				() => client.GetList<AccountGroup>(filter: "AccountGroupID eq '" + accountGroupId + "'"));
		}
	}
}

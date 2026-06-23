using System.Collections.Generic;

namespace IntegrationDiagnostics.Runner
{
	internal static class ScenarioCatalog
	{
		public static IReadOnlyList<IDiagnosticScenario> All()
		{
			return new IDiagnosticScenario[]
			{
				new AccountGroupScenario(),
				new ChangeOrderClassScenario(),
				new ChangeOrderScenario(),
				new CostCodeScenario(),
				new CompoundFilterScenario(),
				new GetListOptimizationScenario(),
				new LastModifiedDateTimeScenario(),
				new ProFormaScenario(),
				new ProjectBudgetScenario(),
				new ProjectLifecycleScenario(),
				new ProjectTaskLifecycleScenario(),
				new ProjectTemplateScenario(),
				new ProjectTemplateTaskScenario(),
				new ProjectTransactionScenario()
			};
		}
	}
}

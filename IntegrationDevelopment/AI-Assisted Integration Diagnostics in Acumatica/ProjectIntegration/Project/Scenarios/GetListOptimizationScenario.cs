using System.Collections.Generic;
using System.Linq;

using Acumatica.Default_22_200_001.Model;
using Acumatica.RESTClient.Client;
using Acumatica.RESTClient.ContractBasedApi;

namespace IntegrationDiagnostics.Runner
{
	internal sealed class GetListOptimizationScenario : IDiagnosticScenario
	{
		public string Name
		{
			get { return "get-list-optimizations"; }
		}

		public void Run(ApiClient client, ScenarioContext context)
		{
			context.Step(
				"GET cost codes optimized vs regular",
				() =>
				{
					var optimized = client.GetList<CostCode>(
						top: 20,
						customHeaders: new Dictionary<string, string> { { "PX-CbApiBehavior", "FASTFAIL" } });

					var regular = client.GetList<CostCode>(
						top: 20,
						customHeaders: new Dictionary<string, string> { { "PX-CbApiBehavior", "NOTOPTIMIZE" } })
						.Where(item => item.CostCodeID != "")
						.ToList();

					return new
					{
						OptimizedCount = optimized.Count,
						RegularCount = regular.Count,
						Optimized = optimized,
						Regular = regular
					};
				});

			context.Step(
				"GET representative project endpoint lists",
				() => new
				{
					AccountGroups = client.GetList<AccountGroup>(top: 20),
					ChangeOrderClasses = client.GetList<ChangeOrderClass>(top: 20),
					Projects = client.GetList<Project>(top: 20),
					ProjectTemplates = client.GetList<ProjectTemplate>(top: 20)
				});
		}
	}
}

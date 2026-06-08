using System;

namespace IntegrationDiagnostics.Runner
{
	internal static class Program
	{
		private static int Main()
		{
			var writer = new JsonScenarioWriter(DiagnosticsConfig.OutputDirectory);
			var hasFailures = false;

			using (var session = new AcumaticaSession())
			{
				Console.WriteLine("Connecting to " + DiagnosticsConfig.BaseUrl);
				var login = new ScenarioContext("session-login");
				session.SetCurrentScenario(login);

				try
				{
					login.Step("Login", () =>
					{
						session.LoginAsync().GetAwaiter().GetResult();
						return new
						{
							DiagnosticsConfig.BaseUrl,
							DiagnosticsConfig.Username,
							DiagnosticsConfig.Company
						};
					});
					login.Complete();
				}
				catch (Exception exception)
				{
					login.Fail(exception);
					writer.Write(login);
					Console.Error.WriteLine("Login failed: " + exception.GetBaseException().Message);
					return 1;
				}

				writer.Write(login);

				foreach (var scenario in ScenarioCatalog.All())
				{
					Console.WriteLine("Running " + scenario.Name);
					var context = new ScenarioContext(scenario.Name);
					session.SetCurrentScenario(context);

					try
					{
						scenario.Run(session.Client, context);
						context.Complete();
					}
					catch (Exception exception)
					{
						hasFailures = true;
						context.Fail(exception);
						Console.Error.WriteLine(scenario.Name + " failed: " + exception.GetBaseException().Message);
					}
					finally
					{
						writer.Write(context);
					}
				}
			}

			Console.WriteLine("Scenario JSON output: " + writer.OutputDirectory);
			return hasFailures ? 1 : 0;
		}
	}
}

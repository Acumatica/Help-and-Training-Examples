using System;
using System.Threading.Tasks;

using Acumatica.RESTClient.AuthApi;
using Acumatica.RESTClient.Client;

namespace IntegrationDiagnostics.Runner
{
	internal sealed class AcumaticaSession : IDisposable
	{
		private ScenarioContext _currentScenario;

		public ApiClient Client { get; private set; }

		public AcumaticaSession()
		{
			Client = new ApiClient(
				DiagnosticsConfig.BaseUrl,
				timeout: 360000,
				requestInterceptor: request =>
				{
					if (_currentScenario != null)
					{
						_currentScenario.AddRequest(request);
					}
				},
				responseInterceptor: response =>
				{
					if (_currentScenario != null)
					{
						_currentScenario.AddResponse(response);
					}
				},
				ignoreSslErrors: true);
		}

		public void SetCurrentScenario(ScenarioContext scenario)
		{
			_currentScenario = scenario;
		}

		public Task LoginAsync()
		{
#pragma warning disable 618
			return Client.LoginAsync(DiagnosticsConfig.Username, DiagnosticsConfig.Password, tenant: DiagnosticsConfig.Company);
#pragma warning restore 618
		}

		public void Dispose()
		{
			if (Client == null)
			{
				return;
			}

			try
			{
				Client.TryLogout();
			}
			catch
			{
				// Keep shutdown quiet; each scenario file already contains the useful diagnostic data.
			}
		}
	}
}

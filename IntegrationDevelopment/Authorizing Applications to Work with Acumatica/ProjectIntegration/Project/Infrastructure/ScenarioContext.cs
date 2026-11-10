using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;

namespace IntegrationDiagnostics.Runner
{
	internal sealed class ScenarioContext
	{
		private readonly List<ScenarioStep> _steps = new List<ScenarioStep>();
		private readonly List<HttpExchange> _http = new List<HttpExchange>();
		private readonly Stopwatch _stopwatch = new Stopwatch();

		public string ScenarioName { get; private set; }
		public string StartedAtUtc { get; private set; }
		public string CompletedAtUtc { get; private set; }
		public long ElapsedMs { get; private set; }
		public string Status { get; private set; }
		public string Error { get; private set; }

		public IEnumerable<ScenarioStep> Steps
		{
			get { return _steps; }
		}

		public IEnumerable<HttpExchange> Http
		{
			get { return _http; }
		}

		public ScenarioContext(string scenarioName)
		{
			ScenarioName = scenarioName;
			Status = "Running";
			StartedAtUtc = Timestamp();
			_stopwatch.Start();
		}

		public T Step<T>(string name, Func<T> action)
		{
			var stopwatch = Stopwatch.StartNew();
			try
			{
				var result = action();
				stopwatch.Stop();

				_steps.Add(new ScenarioStep
				{
					Name = name,
					Status = "Success",
					ElapsedMs = stopwatch.ElapsedMilliseconds,
					Result = result
				});

				return result;
			}
			catch (Exception exception)
			{
				stopwatch.Stop();

				_steps.Add(new ScenarioStep
				{
					Name = name,
					Status = "Failure",
					ElapsedMs = stopwatch.ElapsedMilliseconds,
					Error = exception.ToString()
				});

				throw;
			}
		}

		public void AddRequest(HttpRequestMessage request)
		{
			var body = request.Content == null ? null : request.Content.ReadAsStringAsync().Result;
			_http.Add(new HttpExchange
			{
				TimestampUtc = Timestamp(),
				Direction = "Request",
				Method = request.Method.Method,
				Url = request.RequestUri == null ? null : request.RequestUri.ToString(),
				Body = Sanitize(body)
			});
		}

		public void AddResponse(HttpResponseMessage response)
		{
			var body = response.Content == null ? null : response.Content.ReadAsStringAsync().Result;
			_http.Add(new HttpExchange
			{
				TimestampUtc = Timestamp(),
				Direction = "Response",
				Method = response.RequestMessage == null ? null : response.RequestMessage.Method.Method,
				Url = response.RequestMessage == null || response.RequestMessage.RequestUri == null ? null : response.RequestMessage.RequestUri.ToString(),
				StatusCode = (int)response.StatusCode,
				ReasonPhrase = response.ReasonPhrase,
				Body = Sanitize(body)
			});
		}

		public void Complete()
		{
			_stopwatch.Stop();
			Status = "Success";
			CompletedAtUtc = Timestamp();
			ElapsedMs = _stopwatch.ElapsedMilliseconds;
		}

		public void Fail(Exception exception)
		{
			_stopwatch.Stop();
			Status = "Failure";
			Error = exception.ToString();
			CompletedAtUtc = Timestamp();
			ElapsedMs = _stopwatch.ElapsedMilliseconds;
		}

		public object ToJsonModel()
		{
			return new
			{
				Scenario = ScenarioName,
				Status,
				StartedAtUtc,
				CompletedAtUtc,
				ElapsedMs,
				Error,
				Steps = _steps,
				Http = _http
			};
		}

		private static string Sanitize(string value)
		{
			if (String.IsNullOrEmpty(value))
			{
				return value;
			}

			return value.Replace(DiagnosticsConfig.Password, "***");
		}

		private static string Timestamp()
		{
			return DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture);
		}
	}

	internal sealed class ScenarioStep
	{
		public string Name { get; set; }
		public string Status { get; set; }
		public long ElapsedMs { get; set; }
		public object Result { get; set; }
		public string Error { get; set; }
	}

	internal sealed class HttpExchange
	{
		public string TimestampUtc { get; set; }
		public string Direction { get; set; }
		public string Method { get; set; }
		public string Url { get; set; }
		public int? StatusCode { get; set; }
		public string ReasonPhrase { get; set; }
		public string Body { get; set; }
	}
}

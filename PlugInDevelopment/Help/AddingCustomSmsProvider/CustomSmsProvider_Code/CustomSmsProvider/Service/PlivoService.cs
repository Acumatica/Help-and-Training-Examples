using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PX.SmsProvider.Plivo
{
    public class PlivoService
    {
        private const string PLIVO_BASE_URI = "https://api.plivo.com/v1/Account";
        private static readonly HttpClient _httpClient;

        private readonly string m_sAuthID;
        private readonly string m_sAuthToken;

        public PlivoService(string AuthID, string AuthToken)
        {
            m_sAuthID = AuthID;
            m_sAuthToken = AuthToken;
        }

        static PlivoService()
        {
            _httpClient = new HttpClient(
                new HttpClientHandler
                {
                    SslProtocols = System.Security.Authentication.SslProtocols.Tls12,
                })
            {
                DefaultRequestHeaders =
                {
                    Accept = { MediaTypeWithQualityHeaderValue.Parse("text/json") }
                }
            };
        }

        private string Authentication => Convert.ToBase64String(Encoding.UTF8.GetBytes(m_sAuthID + ":" + m_sAuthToken));

        public async Task<PlivoResponse> PostAsync(List<KeyValuePair<string, string>> liParams, CancellationToken cancellation)
        {
            string requestUri = String.Format(@"{0}/{1}/Message/", PLIVO_BASE_URI, m_sAuthID);
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri(requestUri)))
            {
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/json"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Authentication);
                request.Content = new FormUrlEncodedContent(liParams);

                var response = await _httpClient
                    .SendAsync(request, cancellation).ConfigureAwait(false);

                using (var reader = new StreamReader(await response.Content.ReadAsStreamAsync().ConfigureAwait(false)))
                {
                    PlivoResponse twResponse = new PlivoResponse(response.StatusCode, await reader.ReadToEndAsync().ConfigureAwait(false));

                    if (response.IsSuccessStatusCode)
                    {
                        return twResponse;
                    }

                    JObject jo = JObject.Parse(twResponse.Content);
                    JToken jError = jo?.PropertyValues().Where(x => x?.Path?.ToUpper() == "ERROR")?.FirstOrDefault();

                    if (jError != null)
                    {
                        throw new Exception(jError.ToString());
                    }
                    string strResponse = Environment.NewLine + String.Join(Environment.NewLine, JsonConvert.DeserializeObject<Dictionary<string, string>>(twResponse.Content).ToArray());
                    throw new Exception(strResponse);
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using PX.Api.Webhooks;
using PX.Data;
using PX.Objects.CR;
using PX.Objects.PM;

namespace TogglWebhook
{
    public class TogglWebhookHandler : IWebhookHandler
    {
        private static readonly JsonSerializer Serializer = new JsonSerializer();
        public async Task HandleAsync(WebhookContext context, CancellationToken cancellation)
        {
            if (!context.Request.Headers.TryGetValue(HeaderNames.Authorization, out var authValue))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            if (authValue != "Bearer token")
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
            }

            using (var scope = GetAdminScope())
            {
                using (var jr = new JsonTextReader(context.Request.CreateTextReader()))
                {
                    var payload = Serializer.Deserialize<Dictionary<String, Object>>(jr);
                    if (payload == null)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        return;
                    }
                    var graph = PXGraph.CreateInstance<TimeEntry>();
                    var ta = graph.Items.Insert(new PMTimeActivity()
                    {
                        Date = Convert.ToDateTime(payload["at"]).ToLocalTime(),
                        TimeSpent = Convert.ToInt32(payload["duration"]),
                        OwnerID = PXAccess.GetContactID(),
                        Summary = "Test time entry"
                    });
                    graph.Items.Cache.SetValueExt<PMTimeActivity.projectID>(ta, "X");
                    graph.Items.Cache.SetValueExt(ta, "NoteText", "Created from Toggl");
                    graph.Actions.PressSave();
                    using (var w = context.Response.CreateTextWriter())
                    {
                        Serializer.Serialize(w, new
                        {
                            echo = payload,
                            ts = DateTimeOffset.Now
                        });
                    }
                }
            }
        }
        private IDisposable GetAdminScope()
        {
            var userName = "gibbs@01_U100";
            return new PXLoginScope(userName);
        }
    }
}

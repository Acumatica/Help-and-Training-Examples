using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Newtonsoft.Json.Linq;
using PX.Data;
using PX.Data.Webhooks;
using PX.Objects.CR;
using PX.Objects.PM;

namespace ToggleIntegration
{
    public class ToggleWebhookHandler:IWebhookHandler
    {
        public async Task<System.Web.Http.IHttpActionResult> ProcessRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            using (var scope = GetAdminScope())
            {
                var graph = PXGraph.CreateInstance<TimeEntry>();
                var timeEntry = await request.Content.ReadAsAsync<dynamic>(cancellationToken);
                int duration = timeEntry.duration;
                var ownerId = 2892;
                DateTimeOffset date = timeEntry.at;
                string summary = timeEntry.description;
                string project = timeEntry.project?.name??"X";
                var ta = graph.Items.Insert(new PMTimeActivity() { Date = date.LocalDateTime, TimeSpent = duration, OwnerID = new Guid("B5344897-037E-4D58-B5C3-1BDFD0F47BF9"), Summary = summary});
                graph.Items.Cache.SetValueExt<PMTimeActivity.projectID>(ta, project);
                graph.Items.Cache.SetValueExt(ta, "NoteText", "Created from Toggle");
                graph.Actions.PressSave();
            }
            return new OkResult(request);
        }

        private IDisposable GetAdminScope()
        {
            var userName = "admin";
            if (PXDatabase.Companies.Length > 0)
            {
                var company = PXAccess.GetCompanyName();
                if (string.IsNullOrEmpty(company))
                {
                    company = PXDatabase.Companies[0];
                }
                userName = userName + "@" + company;
            }
            return new PXLoginScope(userName);
        }
    }


}

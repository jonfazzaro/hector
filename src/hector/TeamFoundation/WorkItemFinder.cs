namespace hector.TeamFoundation {
    using Hector;
    using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
    using Microsoft.VisualStudio.Services.Common;
    using State;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    public class WorkItemFinder : IWorkItemFinder {
        const int _pageSize = 269;
        readonly WorkItemTrackingHttpClient _client;

        public WorkItemFinder(ISessionProvider provider) {
            _client = Client(provider);
        }

        public async Task<IEnumerable<WorkItem>> GetWorkItems(string query) {
            var results = await _client.QueryByWiqlAsync(new Wiql { Query = query });
            return await _client.GetWorkItemsAsync(Ids(results), null, null, WorkItemExpand.Fields);
        }

        private static IEnumerable<int> Ids(WorkItemQueryResult results) {
            return results.WorkItems.Select(i => i.Id)
                .Take(_pageSize);
        }

        private WorkItemTrackingHttpClient Client(ISessionProvider provider) {
            return new WorkItemTrackingHttpClient(
                new Uri(provider.Session.Url),
                    new WindowsCredential(
                        new NetworkCredential(
                            provider.Session.Username,
                            provider.Session.Password)));
        }
    }
}

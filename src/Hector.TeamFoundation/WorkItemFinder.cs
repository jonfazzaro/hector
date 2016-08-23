namespace Hector.TeamFoundation {
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
        const int _pageSize = 200;
        readonly WorkItemTrackingHttpClient _client;

        public WorkItemFinder(ISessionProvider provider) {
            _client = Client(provider);
        }

        public async Task<IEnumerable<WorkItem>> GetWorkItems(string query) {
            var results = await _client.QueryByWiqlAsync(new Wiql { Query = query });
            return await GetWorkItems(results);
        }

        private async Task<IEnumerable<WorkItem>> GetWorkItems(WorkItemQueryResult results) {
            var page = 0;
            var items = new List<WorkItem>();
            while ((page * _pageSize) < results.WorkItems.Count()) {
                items.AddRange(await GetPage(page++, results));
            }

            return items;
        }

        private async Task<IEnumerable<WorkItem>> GetPage(int page, WorkItemQueryResult results) {
            var index = page * _pageSize;
            var ids = Ids(results, index, _pageSize);
            return await _client.GetWorkItemsAsync(ids, null, null, WorkItemExpand.Fields);
        }

        private static IEnumerable<int> Ids(WorkItemQueryResult results, int start, int count) {
            return results.WorkItems.Select(i => i.Id)
                .Skip(start)
                .Take(count);
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

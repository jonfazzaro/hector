namespace hector.Hector {
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

    public interface IWorkItemFinder {
        Task<IEnumerable<WorkItem>> GetWorkItems(string query);
    }
}
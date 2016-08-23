namespace Hector {
    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IWorkItemFinder {
        Task<IEnumerable<WorkItem>> GetWorkItems(string query);
    }
}
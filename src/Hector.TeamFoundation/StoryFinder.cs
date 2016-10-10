namespace Hector.TeamFoundation {
    using Stories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class StoryFinder : IStoryFinder {
        private readonly IWorkItemFinder _finder;

        public StoryFinder(IWorkItemFinder finder) {
            _finder = finder;
        }

        public async Task<IEnumerable<Story>> FindStories(string projectName, string areaName = null) {
            var wiql = string.Format(@"SELECT System.ID FROM WorkItems WHERE 
                                    [System.TeamProject] = '{0}' AND
                                    [System.AreaPath] UNDER '{1}' AND
                                    [System.WorkItemType] IN('User Story', 'Product Backlog Item')
                                    ORDER BY System.ID DESC",
                                    projectName, FullyQualified(projectName, areaName));
            var workItems = await _finder.GetWorkItems(wiql);
            return workItems.Select(wi => new Story {
                Id = wi.Id.Value,
                Created = wi.Value<DateTime>("System.CreatedDate"),
                Closed = wi.Value<DateTime?>("Microsoft.VSTS.Common.ClosedDate")
            }).ToList();
        }

        private string FullyQualified(string projectName, string areaName) {
            if (string.IsNullOrWhiteSpace(areaName))
                return projectName;

            return string.Join(@"\", projectName, areaName);
        }
    }
}

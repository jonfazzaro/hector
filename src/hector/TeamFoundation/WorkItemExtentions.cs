namespace hector.TeamFoundation {
    using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

    public static class WorkItemExtentions {
        public static T Value<T>(this WorkItem item, string fieldName) {
            if (item.Fields.ContainsKey(fieldName))
                return (T)item.Fields[fieldName];
            return default(T);
        }
    }
}

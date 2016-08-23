namespace Hector.Stories {
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IStoryFinder {
        Task<IEnumerable<Story>> FindStories(string projectName);
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hector.Hector {
    public interface IStoryFinder {
        Task<IEnumerable<Story>> FindStories(string projectName);
    }
}
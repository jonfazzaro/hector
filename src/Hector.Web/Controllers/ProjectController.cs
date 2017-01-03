namespace Hector.Web.Controllers {
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Hector.Stories;
    using Hector.State;
    using System.Net;

    public class ProjectController : Controller {
        readonly IStoryFinder _finder;
        readonly ISessionProvider _provider;

        public ProjectController(IStoryFinder finder, ISessionProvider provider) {
            _finder = finder;
            _provider = provider;
        }

        [Route("project/{projectName}")]
        [Route("project/{projectName}/{areaName}")]
        public async Task<IActionResult> Index(string projectName, string areaName = null, DateTime? from = null, DateTime? to = null) {
            if (_provider.Session == null)
                return RedirectToAction("Signin", "Session");

            ViewBag.IsAuthenticated = true;
            if (string.IsNullOrWhiteSpace(WebUtility.UrlDecode(projectName)))
                return View();

            var stories = await FindStories(projectName, areaName);
            ViewBag.FromDate = from ?? DateTime.Today.AddMonths(-6);
            ViewBag.ToDate = to ?? DateTime.Today.AddMonths(6);
            return View(stories);
        }

        private async Task<System.Collections.Generic.IEnumerable<Story>> FindStories(string name, string area) {
            return await _finder.FindStories(
                WebUtility.UrlDecode(name), 
                WebUtility.UrlDecode(area));
        }

        private IActionResult Error() {
            return View();
        }

    }
}

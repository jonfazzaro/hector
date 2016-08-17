namespace hector.Controllers {
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Hector;
    using State;

    public class ProjectController : Controller {
        readonly IStoryFinder _finder;
        readonly ISessionProvider _provider;

        public ProjectController(IStoryFinder finder, ISessionProvider provider) {
            _finder = finder;
            _provider = provider;
        }

        [Route("project/{name}")]
        public async Task<IActionResult> Index(string name, int monthsFromNow = 6, int monthsAgo = 6) {
            if (_provider.Session == null)
                return RedirectToAction("Signin", "Session");

            ViewBag.IsAuthenticated = true;
            if (string.IsNullOrWhiteSpace(name))
                return View();

            var stories = await _finder.FindStories(name);
            ViewBag.FromDate = DateTime.Today.AddMonths(-monthsAgo);
            ViewBag.ToDate = DateTime.Today.AddMonths(monthsFromNow);
            return View(stories);
        }

        private IActionResult Error() {
            return View();
        }

    }
}

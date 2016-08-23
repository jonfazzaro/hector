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

        [Route("project/{name}")]
        public async Task<IActionResult> Index(string name, DateTime? from = null, DateTime? to = null) {
            if (_provider.Session == null)
                return RedirectToAction("Signin", "Session");

            ViewBag.IsAuthenticated = true;
            if (string.IsNullOrWhiteSpace(WebUtility.UrlDecode(name)))
                return View();

            var stories = await _finder.FindStories(WebUtility.UrlDecode(name));
            ViewBag.FromDate = from ?? DateTime.Today.AddMonths(-6);
            ViewBag.ToDate = to ?? DateTime.Today.AddMonths(6);
            return View(stories);
        }

        private IActionResult Error() {
            return View();
        }

    }
}

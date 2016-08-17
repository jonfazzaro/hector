namespace hector.Controllers {
    using System;
    using Microsoft.AspNetCore.Mvc;
    using State;
    using Web.Models;

    public class SessionController : Controller {
        readonly ISessionProvider _provider;

        public SessionController(ISessionProvider provider) {
            _provider = provider;
            ViewBag.IsAuthenticated = false;
        }

        [Route("signout")]
        public ActionResult Signout() {
            _provider.Session = null;
            return RedirectToAction("Signin", "Session");
        }

        [Route("")]
        [Route("signin")]
        public ActionResult Signin() {
            return View(new SignInViewModel());
        }

        [Route("")]
        [Route("signin")]
        [HttpPost]
        public ActionResult Signin(SignInViewModel model) {
            if (!ModelState.IsValid)
                return View(model);

            try {
                StartSession(model);
                return RedirectToAction("index", "project");
            } catch (Exception) {
                return Error(model);
            }
        }

        [Route("error")]
        private ActionResult Error(IViewModel model) {
            model.HasError = true;
            model.ErrorMessage = "Let's try that again.";
            return View(model);
        }

        private void StartSession(SignInViewModel model) {
            _provider.Session = new UserSession {
                Url = model.Url,
                Username = model.Username,
                Password = model.Password,
                RememberMe = model.RememberMe
            };
        }

    }
}

namespace Hector.Web.State {
    using Hector.State;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    using System.Linq;
    using System.Text;

    public class HttpSessionProvider : ISessionProvider {
        readonly HttpContext _context;

        public HttpSessionProvider(IHttpContextAccessor contextAccessor) {
            _context = contextAccessor.HttpContext;
        }

        public UserSession Session {
            get {
                if (!_context.Session.Keys.Contains(UserSession.Key))
                    _context.Session.Set(UserSession.Key, Bytes(Cookie()));
                return Deserialize<UserSession>(_context.Session.Get(UserSession.Key));
            }

            set {
                _context.Session.Set(UserSession.Key, Bytes(value));
                Cookie(value);
            }
        }

        byte[] Bytes(object thing) {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(thing));
        }

        T Deserialize<T>(byte[] bytes) {
            return Deserialize<T>(Encoding.UTF8.GetString(bytes));
        }

        T Deserialize<T>(string value) {
            return JsonConvert.DeserializeObject<T>(value);
        }

        private UserSession Cookie() {
            if (!_context.Request.Cookies.Keys.Contains(UserSession.Key))
                return null;

            return Deserialize<UserSession>(
                _context.Request.Cookies[UserSession.Key]);
        }

        private void Cookie(UserSession session) {
            if (session == null)
                _context.Response.Cookies.Delete(UserSession.Key);
            else if (session.RememberMe)
                _context.Response.Cookies.Append(UserSession.Key,
                    JsonConvert.SerializeObject(session),
                    new CookieOptions { HttpOnly = true });
        }

    }
}
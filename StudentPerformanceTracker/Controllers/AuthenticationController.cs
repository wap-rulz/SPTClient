using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPTClient.Services;

namespace StudentPerformanceTracker.Controllers
{
    public class AuthenticationController : Controller
    {
        private APIServiceI _apiServiceI;

        public AuthenticationController(APIServiceI apiServiceI)
        {
            _apiServiceI = apiServiceI;
        }


        // GET: AuthenticationController
        [AllowAnonymous]
        public ActionResult Login()
        {
            ViewData["HideNavBar"] = true;
            return View();
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewData["HideNavBar"] = true;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(String username, String password)
        {
            var isAuthenticated = await _apiServiceI.AuthenticateAsync(username, password);

            if (isAuthenticated)
            {
                HttpContext.Session.SetString("Username", username);
                return RedirectToAction("Index", "StudySession");
            }

            ViewBag.Error = "Invalid username or password!";
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(String username, String password)
        {
            await _apiServiceI.RegisterAsync(username, password);
            return RedirectToAction(nameof(Login));
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Remove("Username");
            return RedirectToAction(nameof(Login));
        }
    }
}

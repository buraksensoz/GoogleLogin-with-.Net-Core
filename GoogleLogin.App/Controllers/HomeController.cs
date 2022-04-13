using GoogleLogin.App.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GoogleLogin.App.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task LoginGoogle()
        {

            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                RedirectUri = "/Home/LoggedUserInfo"
            });
        }


        public IActionResult LoggedUserInfo()
        {

            //Get Facebook Data From Cookie
            var userrec = _httpContextAccessor.HttpContext.User;
            var model = new UserModel()
            {
                Email = userrec.FindFirstValue(ClaimTypes.Email),
                UserName = userrec.FindFirstValue(ClaimTypes.Name),
                Identifier = userrec.FindFirstValue(ClaimTypes.NameIdentifier),
                Born = userrec.FindFirstValue(ClaimTypes.DateOfBirth),
                PhotoUrl = userrec.FindFirstValue("urn:google:picture")

            };

            return View(model);
        }

        public async Task<IActionResult> LogOut()
        {
            //Signout FaceLogin..
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("index");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

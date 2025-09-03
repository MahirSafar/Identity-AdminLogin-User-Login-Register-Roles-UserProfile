using Microsoft.AspNetCore.Mvc;

namespace Pustok.App.Controllers
{
    public class BasketController : Controller
    {
        public IActionResult Index()
        {
            //SetCookie();
            //var value = GetCookie();
            return View();
        }
        public void SetCookie()
        {
            string cookieValue = "Pustok";
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Append("name", cookieValue,options);
        }
        public string GetCookie()
        {
            string cookieValue = Request.Cookies["name"];
            return cookieValue;
        }
        public IActionResult RemoveCookie()
        {
            Response.Cookies.Delete("name");
            return RedirectToAction("Index");
        }
        public IActionResult SessionSet()
        {
            HttpContext.Session.SetString("name", "Pustok");
            return Content("Setted");
        }
        public IActionResult SessionGet()
        {
            string value = HttpContext.Session.GetString("name");
            return Content(value);
        }
    }
}

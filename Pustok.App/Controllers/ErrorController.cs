using Microsoft.AspNetCore.Mvc;

namespace Pustok.App.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, the page you requested could not be found";
                    ViewBag.ErrorCode = "404";
                    ViewBag.ErrorTitle = "Page Not Found";
                    break;
                case 500:
                    ViewBag.ErrorMessage = "Sorry, something went wrong on the server";
                    ViewBag.ErrorCode = "500";
                    ViewBag.ErrorTitle = "Server Error";
                    break;
                default:
                    ViewBag.ErrorMessage = "Sorry, something went wrong";
                    ViewBag.ErrorCode = statusCode.ToString();
                    ViewBag.ErrorTitle = "Error";
                    break;
            }

            return View("Error");
        }
    }
}
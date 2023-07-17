using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace Inveasy.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Username"] = "Username";
            ViewData["FirstName"] = "FirstName";
            ViewData["LastName"] = "LastName";
            ViewData["Email"] = "Email";
            return View();
        }
    }
}

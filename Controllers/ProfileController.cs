using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using Newtonsoft.Json.Linq;

namespace Inveasy.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            var session = HttpContext.Session.GetString("User");
            var json = JObject.Parse(session);
        
            ViewData["Username"] = json["Username"].ToString();
            ViewData["FirstName"] = json["Name"].ToString();
            ViewData["LastName"] = json["Surname"].ToString();
            ViewData["Email"] = json["Email"].ToString();
            return View();
        }
    }
}

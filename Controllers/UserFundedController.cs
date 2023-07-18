using Inveasy.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Inveasy.Controllers
{
    public class UserFundedController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

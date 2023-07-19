using Inveasy.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Inveasy.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

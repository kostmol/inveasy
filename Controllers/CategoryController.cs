using Inveasy.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Inveasy.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

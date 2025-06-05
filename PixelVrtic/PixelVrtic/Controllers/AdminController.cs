using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PixelVrtic.Controllers
{
    [Authorize]

    public class AdminController : Controller
    {
        // GET: /Admin/Dashboard
        [Authorize(Roles = "Administrator")]

        public IActionResult Dashboard()
        {
            return View();
        }
    }
}

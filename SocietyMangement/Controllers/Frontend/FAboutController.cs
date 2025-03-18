using Microsoft.AspNetCore.Mvc;

namespace SocietyMangement.Controllers.Frontend
{
    [CheckAccess]
    public class FAboutController : Controller
    {
        public IActionResult About()
        {
            return View();
        }
    }
}

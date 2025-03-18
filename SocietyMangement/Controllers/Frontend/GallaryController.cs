using Microsoft.AspNetCore.Mvc;

namespace SocietyMangement.Controllers.Frontend
{
    [CheckAccess]
    
    public class GallaryController: Controller
    {

        public IActionResult GallaryList()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace SL.Controllers
{
    public class CineController : Controller
    {
        public IActionResult Index()
        {
            return Ok();
        }
    }
}

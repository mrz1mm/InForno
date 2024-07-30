using InForno.Models;
using Microsoft.AspNetCore.Mvc;

namespace InForno.Controllers
{
    public class CustomerController : Controller
    {
        private readonly InFornoDbContext _context;
        public CustomerController(InFornoDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}

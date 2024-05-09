
using Ab_105_Pronia.DAL;
using Ab_105_Pronia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Ab_105_Pronia.Controllers
{
    public class HomeController : Controller
    {
        AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> products = await _context.Products.ToListAsync();
            return View(products);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var product = _context.Products.Include(x=>x.Category).FirstOrDefault(x=>x.Id==id);
            ViewBag.Related = _context.Products.Where(x => x.CategoryId == product.CategoryId&&x.Id!=product.Id).ToList();
            return View(product);
        }

      
    }
}
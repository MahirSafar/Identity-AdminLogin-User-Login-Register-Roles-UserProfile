using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.App.DAL.Context;
using Pustok.App.ViewModels;

namespace Pustok.App.Controllers
{
    public class HomeController(PustokDbContext pustokDbContext) : Controller
    {
        public IActionResult Index()
        {
            HomeVM homeVM = new()
            {
                Sliders = pustokDbContext.Sliders.ToList(),
                FeaturedBooks = pustokDbContext.Books
                    .Include(b => b.Author)
                    .Where(b => b.IsFeatured)
                    .ToList(),
                NewBooks = pustokDbContext.Books
                    .Include(b => b.Author)
                    .Where(b => b.IsNew)
                    .ToList(),
                DicountBooks = pustokDbContext.Books
                    .Include(b => b.Author)
                    .Where(b => b.DiscountPercentage > 0)
                    .ToList(),
                Featured = pustokDbContext.Featured.ToList()
            };
            return View(homeVM);
        }
    }
}
    
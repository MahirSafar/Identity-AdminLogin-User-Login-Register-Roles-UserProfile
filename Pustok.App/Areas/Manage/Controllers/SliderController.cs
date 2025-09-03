using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.App.DAL.Context;
using Pustok.App.Extensions;
using Pustok.App.Models;

namespace Pustok.App.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class SliderController(PustokDbContext pustokDbContext) : Controller
    {
        public IActionResult Index()
        {
            var sliders = pustokDbContext.Sliders.ToList();
            return View(sliders);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Slider slider)
        {
            if (!ModelState.IsValid)
                return View();
            var file = slider.File;
            if (slider.File == null)
            {
                ModelState.AddModelError("File", "Image is required.");
                return View();
            }

            slider.ImageUrl = file.SaveFile("bg-images");
            slider.CreatedAt = DateTime.Now;

            pustokDbContext.Sliders.Add(slider);
            pustokDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var slider = pustokDbContext.Sliders.Find(id);
            if (slider == null) return NotFound();
            pustokDbContext.Sliders.Remove(slider);
            pustokDbContext.SaveChanges();

            FileManager.DeleteFile("bg-images", slider.ImageUrl);
            return Ok();
        }
        public IActionResult Edit(int id)
        {
            var slider = pustokDbContext.Sliders.Find(id);
            if (slider == null) return NotFound();
            return View(slider);
        }
        [HttpPost]
        public IActionResult Edit(Slider slider)
        {
            if (!ModelState.IsValid)
                return View();
            var existSlider = pustokDbContext.Sliders.Find(slider.Id);
            if (existSlider == null) return NotFound();
            var file = slider.File;
            if (file != null)
            {
                FileManager.DeleteFile("bg-images", existSlider.ImageUrl);
                existSlider.ImageUrl = file.SaveFile("bg-images");
            }
            existSlider.Title = slider.Title;
            existSlider.Description = slider.Description;
            existSlider.ButtonText = slider.ButtonText;
            existSlider.ButtonLink = slider.ButtonLink;
            existSlider.Order = slider.Order;
            existSlider.UpdatedAt = DateTime.Now;
            pustokDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Detail(int id)
        {
            var slider = pustokDbContext.Sliders
                .Find(id);
            if (slider == null) return NotFound();
            return PartialView("_SliderDetailModal", slider);
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.App.DAL.Context;
using Pustok.App.ViewModels;

namespace Pustok.App.Controllers
{
    public class BookController(PustokDbContext pustokDbContext,IMapper mapper) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Detail(int? id)
        {
            if (id == null)
                return NotFound("Book ID cannot be null");

            var book = pustokDbContext.Books
                .Include(b => b.BookTags).ThenInclude(bt => bt.Tag)
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .Include(b => b.BookImages)
                .FirstOrDefault(b => b.Id == id);

            if (book == null)
                return NotFound($"Book with ID {id} not found");

            BookDetailVM bookDetailVM = new()
            {
                Book = book,
                RelatedBooks = pustokDbContext.Books
                    .Where(b => b.GenreId == book.GenreId && b.Id != book.Id)
                    .Include(b => b.Author)
                    .Take(4)
                    .ToList()
            };
            return View(bookDetailVM);
        }
        //public IActionResult Test()
        //{
        //    var books = pustokDbContext.Books
        //        .Include(b => b.Author)
        //        .Include(b => b.Genre)
        //        .ToList();
        //    return PartialView("_BooksPartial",books);
        //}
        public IActionResult BookModal(int? id)
        {
            if(id is null)
                return NotFound("Book ID cannot be null");
            var book = pustokDbContext.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .Include(b=>b.BookImages)
                .Include(b=>b.BookTags)
                .ThenInclude(bt => bt.Tag)
                .FirstOrDefault(b => b.Id == id);

            if (book is null)
                return NotFound("No books found matching the search criteria");
            return PartialView("_BookModal", book);
        }
        public IActionResult Test(int? id)
        {
            if (id is null)
                return NotFound("Book ID cannot be null");
            var book = pustokDbContext.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .Include(b => b.BookImages)
                .Include(b => b.BookTags)
                .ThenInclude(bt => bt.Tag)
                .FirstOrDefault(b => b.Id == id);

            if (book is null)
                return NotFound("No books found matching the search criteria");

            BookTestVM bookTestVM = mapper.Map<BookTestVM>(book);
            return Json(bookTestVM);
        }
    }
}

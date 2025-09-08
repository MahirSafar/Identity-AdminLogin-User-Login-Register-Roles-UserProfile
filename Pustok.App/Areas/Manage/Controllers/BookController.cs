using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Pustok.App.DAL.Context;
using Pustok.App.Extensions;
using Pustok.App.Models;
using Pustok.App.Services;
using System.Drawing;
using System.Web;

namespace Pustok.App.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class BookController(PustokDbContext pustokDbContext) : Controller
    {
        public IActionResult Index()
        {
            var books = pustokDbContext.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .ToList();
            return View(books);
        }
        public IActionResult Delete(int id)
        {
            var book = pustokDbContext.Books
                .Include(m => m.BookImages)
                .FirstOrDefault(m => m.Id == id);
            if (book == null) return NotFound();
            pustokDbContext.Books.Remove(book);
            pustokDbContext.SaveChanges();

            FileManager.DeleteFile("products", book.MainImageUrl);
            FileManager.DeleteFile("products", book.HoverImageUrl);
            foreach (var item in book.BookImages)
                FileManager.DeleteFile("products", item.ImageUrl);

            return Ok();
        }
        public IActionResult Create()
        {
            ViewBag.Authors = pustokDbContext.Authors.ToList();
            ViewBag.Genres = pustokDbContext.Genres.ToList();
            ViewBag.Tags = pustokDbContext.Tags.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            ViewBag.Authors = pustokDbContext.Authors.ToList();
            ViewBag.Genres = pustokDbContext.Genres.ToList();
            ViewBag.Tags = pustokDbContext.Tags.ToList();

            if (!ModelState.IsValid) return View(book);

            if (!pustokDbContext.Authors.Any(x => x.Id == book.AuthorId))
            {
                ModelState.AddModelError("AuthorId", "Author not found.");
                return View(book);
            }
            if (!pustokDbContext.Genres.Any(x => x.Id == book.GenreId))
            {
                ModelState.AddModelError("GenreId", "Genre not found.");
                return View(book);
            }

            foreach (var item in book.TagsId)
            {
                if (!pustokDbContext.Tags.Any(x => x.Id == item))
                {
                    ModelState.AddModelError("TagId", "Tag not found.");
                    return View(book);
                }
            }

            // Assign Tags
            foreach (var item in book.TagsId)
            {
                var bookTag = new BookTag
                {
                    TagId = item,
                    BookId = book.Id
                };
                book.BookTags.Add(bookTag);
            }

            if (book.MainPhoto == null)
            {
                ModelState.AddModelError("MainPhoto", "Main photo is required.");
                return View(book);
            }
            if (book.HoverPhoto == null)
            {
                ModelState.AddModelError("HoverPhoto", "Hover photo is required.");
                return View(book);
            }

            // Save main and hover photos
            book.MainImageUrl = book.MainPhoto.SaveFile("products");
            book.HoverImageUrl = book.HoverPhoto.SaveFile("products");

            // Save additional photos
            if (book.Photos != null)
            {
                foreach (var photo in book.Photos)
                {
                    var bookImage = new BookImage
                    {
                        ImageUrl = photo.SaveFile("products")
                    };
                    book.BookImages.Add(bookImage);
                }
            }

            pustokDbContext.Books.Add(book);
            pustokDbContext.SaveChanges();

            var telegram = new TelegramService();
            var messageText = $"{User.Identity.Name} created: {book.Title}";
            var buttonUrl = $"http://localhost:5026/Manage/Book/{book.Id}";

            await telegram.SendMessageAsync(messageText, buttonUrl);


            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var book = pustokDbContext.Books
                .Include(b => b.BookTags)
                .Include(b => b.BookImages)
                .FirstOrDefault(b => b.Id == id);
            if (book == null) return NotFound();
            ViewBag.Authors = pustokDbContext.Authors.ToList();
            ViewBag.Genres = pustokDbContext.Genres.ToList();
            ViewBag.Tags = pustokDbContext.Tags.ToList();
            book.TagsId = book.BookTags.Select(bt => bt.TagId).ToList();
            return View(book);
        }
        public IActionResult DeleteImage(int id)
        {
            var bookImage = pustokDbContext.BookImages.FirstOrDefault(bi => bi.Id == id);
            if (bookImage == null) return NotFound();
            FileManager.DeleteFile("products", bookImage.ImageUrl);
            pustokDbContext.BookImages.Remove(bookImage);
            pustokDbContext.SaveChanges();
            return RedirectToAction(nameof(Edit), new { id = bookImage.BookId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Book book)
        {
            ViewBag.Authors = pustokDbContext.Authors.ToList();
            ViewBag.Genres = pustokDbContext.Genres.ToList();
            ViewBag.Tags = pustokDbContext.Tags.ToList();
            var existBook = pustokDbContext.Books
                .Include(b => b.BookTags)
                .Include(b => b.BookImages)
                .FirstOrDefault(b => b.Id == book.Id);
            if (existBook == null) return NotFound();
            if (!ModelState.IsValid) return View(existBook);
            if (!pustokDbContext.Authors.Any(x => x.Id == book.AuthorId))
            {
                ModelState.AddModelError("AuthorId", "Author not found.");
                return View(existBook);
            }
            if (!pustokDbContext.Genres.Any(x => x.Id == book.GenreId))
            {
                ModelState.AddModelError("GenreId", "Genre not found.");
                return View(existBook);
            }
            foreach (var item in book.TagsId)
            {
                if (!pustokDbContext.Tags.Any(x => x.Id == item))
                {
                    ModelState.AddModelError("TagId", "Tag not found.");
                    return View(existBook);
                }
            }
            existBook.BookTags.RemoveAll(bt => !book.TagsId.Contains(bt.TagId));
            var existTagIds = existBook.BookTags.Select(bt => bt.TagId).ToList();
            var newTagIds = book.TagsId.Except(existTagIds).ToList();

            foreach (var item in newTagIds)
            {
                var bookTag = new BookTag
                {
                    TagId = item,
                    BookId = book.Id
                };
                existBook.BookTags.Add(bookTag);
            }
            if (book.MainPhoto != null)
            {
                FileManager.DeleteFile("products", existBook.MainImageUrl);
                existBook.MainImageUrl = book.MainPhoto.SaveFile("products");
            }
            if (book.HoverPhoto != null)
            {
                FileManager.DeleteFile("products", existBook.HoverImageUrl);
                existBook.HoverImageUrl = book.HoverPhoto.SaveFile("products");
            }
            if (book.Photos != null)
            {
                foreach (var photo in book.Photos)
                {
                    var bookImage = new BookImage
                    {
                        ImageUrl = photo.SaveFile("products")
                    };
                    existBook.BookImages.Add(bookImage);
                }
            }
            existBook.Title = book.Title;
            existBook.Description = book.Description;
            existBook.Price = book.Price;
            existBook.DiscountPercentage = book.DiscountPercentage;
            existBook.IsFeatured = book.IsFeatured;
            existBook.IsNew = book.IsNew;
            existBook.Code = book.Code;
            existBook.Title = book.Title;
            existBook.AuthorId = book.AuthorId;
            existBook.GenreId = book.GenreId;
            existBook.StockCount = book.StockCount;
            pustokDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Detail(int id)
        {
            var book = pustokDbContext.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .Include(b => b.BookTags).ThenInclude(bt => bt.Tag)
                .Include(b => b.BookImages)
                .FirstOrDefault(b => b.Id == id);
            if (book == null) return NotFound();
            return View(book);
        }
        public IActionResult ExportData()
        {
            var books = pustokDbContext.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .ToList();

            using (var packages = new ExcelPackage())
            {
                var worksheet = packages.Workbook.Worksheets.Add("Books");

                // Headers
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Title";
                worksheet.Cells[1, 3].Value = "Author";
                worksheet.Cells[1, 4].Value = "Genre";
                worksheet.Cells[1, 5].Value = "Price";
                worksheet.Cells[1, 6].Value = "Discount Percentage";
                worksheet.Cells[1, 7].Value = "Is Featured";
                worksheet.Cells[1, 8].Value = "Is New";
                worksheet.Cells[1, 9].Value = "Code";
                worksheet.Cells[1, 10].Value = "Stock Count";
                worksheet.Cells[1, 11].Value = "Created At";

                for (int i = 0; i < books.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = books[i].Id;
                    worksheet.Cells[i + 2, 2].Value = books[i].Title;
                    worksheet.Cells[i + 2, 3].Value = books[i].Author.Name;
                    worksheet.Cells[i + 2, 4].Value = books[i].Genre.Name;
                    worksheet.Cells[i + 2, 5].Value = books[i].Price;
                    worksheet.Cells[i + 2, 6].Value = books[i].DiscountPercentage;
                    worksheet.Cells[i + 2, 7].Value = books[i].IsFeatured ? "Yes" : "No";
                    worksheet.Cells[i + 2, 8].Value = books[i].IsNew ? "Yes" : "No";
                    worksheet.Cells[i + 2, 9].Value = books[i].Code;
                    worksheet.Cells[i + 2, 10].Value = books[i].StockCount;
                    worksheet.Cells[i + 2, 11].Value = books[i].CreatedAt.ToString("yyyy-MM-dd");
                }

                var stream = new MemoryStream(packages.GetAsByteArray());
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Books.xlsx");
            }
        }
        public IActionResult ImportData()
        {
            return PartialView("_ImportDataPartial");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ImportData(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("File", "Please select a valid Excel file.");
                return PartialView("_ImportDataPartial");
            }
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.First();
                    var rowCount = worksheet.Dimension.Rows;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var title = worksheet.Cells[row, 1].Value?.ToString().Trim();
                        var description = worksheet.Cells[row, 2].Value?.ToString().Trim();
                        var price = decimal.Parse(worksheet.Cells[row, 3].Value?.ToString().Trim() ?? "0");
                        var discountPercentage = decimal.Parse(worksheet.Cells[row, 4].Value?.ToString().Trim() ?? "0");
                        var isFeatured = worksheet.Cells[row, 5].Value?.ToString().Trim().ToLower() == "yes";
                        var isNew = worksheet.Cells[row, 6].Value?.ToString().Trim().ToLower() == "yes";
                        var code = worksheet.Cells[row, 7].Value?.ToString().Trim();
                        var authorName = worksheet.Cells[row, 8].Value?.ToString().Trim();
                        var genreName = worksheet.Cells[row, 9].Value?.ToString().Trim();
                        var stockCount = int.Parse(worksheet.Cells[row, 10].Value?.ToString().Trim() ?? "0");
                        var author = pustokDbContext.Authors.FirstOrDefault(a => a.Name.ToLower() == authorName.ToLower());
                        if (author == null)
                        {
                            author = new Author { Name = authorName };
                            pustokDbContext.Authors.Add(author);
                            pustokDbContext.SaveChanges();
                        }
                        var genre = pustokDbContext.Genres.FirstOrDefault(g => g.Name.ToLower() == genreName.ToLower());
                        if (genre == null)
                        {
                            genre = new Genre { Name = genreName };
                            pustokDbContext.Genres.Add(genre);
                            pustokDbContext.SaveChanges();
                        }
                        var book = new Book
                        {
                            Title = title,
                            Description = description,
                            Price = price,
                            DiscountPercentage = discountPercentage,
                            IsFeatured = isFeatured,
                            IsNew = isNew,
                            Code = code,
                            AuthorId = author.Id,
                            GenreId = genre.Id,
                            StockCount = stockCount,
                            MainImageUrl = "default.jpg",
                            HoverImageUrl = "default.jpg",
                            CreatedAt = DateTime.Now
                        };
                        pustokDbContext.Books.Add(book);
                        pustokDbContext.SaveChanges();
                    }
                }
            }
            return RedirectToAction("Index");
        }
    }
}

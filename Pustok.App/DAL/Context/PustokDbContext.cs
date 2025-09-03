using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pustok.App.Models;

namespace Pustok.App.DAL.Context
{
    public class PustokDbContext(DbContextOptions<PustokDbContext> options) : IdentityDbContext<AppUser>(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PustokDbContext).Assembly);
            base.OnModelCreating(modelBuilder);

            // Seed Authors (5)
            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, Name = "J.K. Rowling" },
                new Author { Id = 2, Name = "George R.R. Martin" },
                new Author { Id = 3, Name = "J.R.R. Tolkien" },
                new Author { Id = 4, Name = "Stephen King" },
                new Author { Id = 5, Name = "Agatha Christie" }
            );

            // Seed Genres (3)
            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Fantasy" },
                new Genre { Id = 2, Name = "Adventure" },
                new Genre { Id = 3, Name = "Mystery" }
            );

            // Seed Books (10) with real names and descriptions
            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "Harry Potter and the Sorcerer's Stone", Description = "A young boy discovers he is a wizard and attends Hogwarts School of Witchcraft and Wizardry.", Price = 10.99m, DiscountPercentage = 0, IsFeatured = true, IsNew = true, Code = "HP1", AuthorId = 1, GenreId = 1, MainImageUrl = "product-1.jpg", HoverImageUrl = "product-details-1.jpg", CreatedAt = new DateTime(2024, 1, 1), StockCount = 10 },
                new Book { Id = 2, Title = "A Game of Thrones", Description = "Noble families vie for control of the Iron Throne in the land of Westeros.", Price = 11.99m, DiscountPercentage = 5, IsFeatured = false, IsNew = true, Code = "GOT1", AuthorId = 2, GenreId = 1, MainImageUrl = "product-2.jpg", HoverImageUrl = "product-details-2.jpg", CreatedAt = new DateTime(2024, 1, 2), StockCount = 8 },
                new Book { Id = 3, Title = "The Hobbit", Description = "Bilbo Baggins embarks on a quest to reclaim a lost Dwarf Kingdom from a dragon.", Price = 12.99m, DiscountPercentage = 10, IsFeatured = true, IsNew = false, Code = "HOBBIT", AuthorId = 3, GenreId = 2, MainImageUrl = "product-3.jpg", HoverImageUrl = "product-details-3.jpg", CreatedAt = new DateTime(2024, 1, 3), StockCount = 12 },
                new Book { Id = 4, Title = "The Shining", Description = "A family heads to an isolated hotel for the winter where a sinister presence the father violence.", Price = 13.99m, DiscountPercentage = 0, IsFeatured = false, IsNew = false, Code = "SHINING", AuthorId = 4, GenreId = 2, MainImageUrl = "product-4.jpg", HoverImageUrl = "product-details-4.jpg", CreatedAt = new DateTime(2024, 1, 4), StockCount = 7 },
                new Book { Id = 5, Title = "Murder on the Orient Express", Description = "Detective Hercule Poirot investigates a murder on a snowbound train.", Price = 14.99m, DiscountPercentage = 15, IsFeatured = true, IsNew = true, Code = "MOTOE", AuthorId = 5, GenreId = 3, MainImageUrl = "product-5.jpg", HoverImageUrl = "product-details-5.jpg", CreatedAt = new DateTime(2024, 1, 5), StockCount = 9 },
                new Book { Id = 6, Title = "Harry Potter and the Chamber of Secrets", Description = "Harry returns for his second year at Hogwarts and faces new dangers.", Price = 15.99m, DiscountPercentage = 0, IsFeatured = false, IsNew = false, Code = "HP2", AuthorId = 1, GenreId = 1, MainImageUrl = "product-6.jpg", HoverImageUrl = "product-details-1.jpg", CreatedAt = new DateTime(2024, 1, 6), StockCount = 10 },
                new Book { Id = 7, Title = "A Clash of Kings", Description = "The Seven Kingdoms are plagued by civil war as rival claimants to the Iron Throne emerge.", Price = 16.99m, DiscountPercentage = 5, IsFeatured = true, IsNew = true, Code = "GOT2", AuthorId = 2, GenreId = 1, MainImageUrl = "product-7.jpg", HoverImageUrl = "product-details-2.jpg", CreatedAt = new DateTime(2024, 1, 7), StockCount = 6 },
                new Book { Id = 8, Title = "The Lord of the Rings: The Fellowship of the Ring", Description = "Frodo Baggins sets out on a journey to destroy the One Ring.", Price = 17.99m, DiscountPercentage = 10, IsFeatured = false, IsNew = false, Code = "LOTR1", AuthorId = 3, GenreId = 2, MainImageUrl = "product-8.jpg", HoverImageUrl = "product-details-3.jpg", CreatedAt = new DateTime(2024, 1, 8), StockCount = 11 },
                new Book { Id = 9, Title = "Carrie", Description = "A bullied high school girl discovers her telekinetic powers with tragic results.", Price = 18.99m, DiscountPercentage = 0, IsFeatured = true, IsNew = true, Code = "CARRIE", AuthorId = 4, GenreId = 2, MainImageUrl = "product-9.jpg", HoverImageUrl = "product-details-4.jpg", CreatedAt = new DateTime(2024, 1, 9), StockCount = 5 },
                new Book { Id = 10, Title = "And Then There Were None", Description = "Ten strangers are invited to an island, where they are killed one by one.", Price = 19.99m, DiscountPercentage = 15, IsFeatured = false, IsNew = false, Code = "ATTWN", AuthorId = 5, GenreId = 3, MainImageUrl = "product-10.jpg", HoverImageUrl = "product-details-5.jpg", CreatedAt = new DateTime(2024, 1, 10), StockCount = 8 }
            );

            // Seed BookImages (use all product and product-details images)
            modelBuilder.Entity<BookImage>().HasData(
                new BookImage { Id = 1, BookId = 1, ImageUrl = "product-1.jpg" },
                new BookImage { Id = 2, BookId = 2, ImageUrl = "product-2.jpg" },
                new BookImage { Id = 3, BookId = 3, ImageUrl = "product-3.jpg" },
                new BookImage { Id = 4, BookId = 4, ImageUrl = "product-4.jpg" },
                new BookImage { Id = 5, BookId = 5, ImageUrl = "product-5.jpg" },
                new BookImage { Id = 6, BookId = 6, ImageUrl = "product-6.jpg" },
                new BookImage { Id = 7, BookId = 7, ImageUrl = "product-7.jpg" },
                new BookImage { Id = 8, BookId = 8, ImageUrl = "product-8.jpg" },
                new BookImage { Id = 9, BookId = 9, ImageUrl = "product-9.jpg" },
                new BookImage { Id = 10, BookId = 10, ImageUrl = "product-10.jpg" },
                new BookImage { Id = 11, BookId = 1, ImageUrl = "product-11.jpg" },
                new BookImage { Id = 12, BookId = 2, ImageUrl = "product-12.jpg" },
                new BookImage { Id = 13, BookId = 3, ImageUrl = "product-13.jpg" },
                new BookImage { Id = 14, BookId = 4, ImageUrl = "product-details-1.jpg" },
                new BookImage { Id = 15, BookId = 5, ImageUrl = "product-details-2.jpg" },
                new BookImage { Id = 16, BookId = 6, ImageUrl = "product-details-3.jpg" },
                new BookImage { Id = 17, BookId = 7, ImageUrl = "product-details-4.jpg" },
                new BookImage { Id = 18, BookId = 8, ImageUrl = "product-details-5.jpg" }
            );

            // Seed Sliders (3 example sliders)
            modelBuilder.Entity<Slider>().HasData(
                new Slider { Id = 1, Title = "Welcome to Pustok", Description = "Best books available!", ImageUrl = "product-1.jpg", ButtonText = "Shop Now", ButtonLink = "/books", Order = 1, CreatedAt = new DateTime(2024, 1, 1) },
                new Slider { Id = 2, Title = "New Arrivals", Description = "Check out the latest books!", ImageUrl = "product-2.jpg", ButtonText = "View New", ButtonLink = "/books/new", Order = 2, CreatedAt = new DateTime(2024, 1, 2) },
                new Slider { Id = 3, Title = "Best Sellers", Description = "Our most popular books!", ImageUrl = "product-3.jpg", ButtonText = "See Bestsellers", ButtonLink = "/books/bestsellers", Order = 3, CreatedAt = new DateTime(2024, 1, 3) }
            );

            // Seed Tags
            modelBuilder.Entity<Tag>().HasData(
                new Tag { Id = 1, Name = "Magic" },
                new Tag { Id = 2, Name = "Classic" },
                new Tag { Id = 3, Name = "Bestseller" },
                new Tag { Id = 4, Name = "Adventure" },
                new Tag { Id = 5, Name = "Mystery" }
            );

            // Seed BookTags
            modelBuilder.Entity<BookTag>().HasData(
                new BookTag { BookId = 1, TagId = 1 },
                new BookTag { BookId = 1, TagId = 3 },
                new BookTag { BookId = 2, TagId = 3 },
                new BookTag { BookId = 2, TagId = 4 },
                new BookTag { BookId = 3, TagId = 2 },
                new BookTag { BookId = 3, TagId = 4 },
                new BookTag { BookId = 4, TagId = 5 },
                new BookTag { BookId = 5, TagId = 5 },
                new BookTag { BookId = 6, TagId = 1 },
                new BookTag { BookId = 7, TagId = 4 },
                new BookTag { BookId = 8, TagId = 2 },
                new BookTag { BookId = 9, TagId = 3 },
                new BookTag { BookId = 10, TagId = 5 }
            );
        }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookImage> BookImages { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Featured> Featured { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BookTag> BookTags { get; set; }
        public DbSet<Setting> Settings { get; set; }
    }
}

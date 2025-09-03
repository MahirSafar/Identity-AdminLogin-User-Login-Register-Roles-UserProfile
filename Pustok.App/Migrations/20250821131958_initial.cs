using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Pustok.App.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sliders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ButtonText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ButtonLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sliders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    IsNew = table.Column<bool>(type: "bit", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false),
                    MainImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HoverImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StockCount = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Books_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookImages_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "J.K. Rowling" },
                    { 2, "George R.R. Martin" },
                    { 3, "J.R.R. Tolkien" },
                    { 4, "Stephen King" },
                    { 5, "Agatha Christie" }
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Fantasy" },
                    { 2, "Adventure" },
                    { 3, "Mystery" }
                });

            migrationBuilder.InsertData(
                table: "Sliders",
                columns: new[] { "Id", "ButtonLink", "ButtonText", "CreatedAt", "Description", "ImageUrl", "Order", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "/books", "Shop Now", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Best books available!", "product-1.jpg", 1, "Welcome to Pustok", null },
                    { 2, "/books/new", "View New", new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Check out the latest books!", "product-2.jpg", 2, "New Arrivals", null },
                    { 3, "/books/bestsellers", "See Bestsellers", new DateTime(2024, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Our most popular books!", "product-3.jpg", 3, "Best Sellers", null }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "Code", "CreatedAt", "Description", "DiscountPercentage", "GenreId", "HoverImageUrl", "IsFeatured", "IsNew", "MainImageUrl", "Price", "StockCount", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, "HP1", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "A young boy discovers he is a wizard and attends Hogwarts School of Witchcraft and Wizardry.", 0m, 1, "product-details-1.jpg", true, true, "product-1.jpg", 10.99m, 10, "Harry Potter and the Sorcerer's Stone", null },
                    { 2, 2, "GOT1", new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Noble families vie for control of the Iron Throne in the land of Westeros.", 5m, 1, "product-details-2.jpg", false, true, "product-2.jpg", 11.99m, 8, "A Game of Thrones", null },
                    { 3, 3, "HOBBIT", new DateTime(2024, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bilbo Baggins embarks on a quest to reclaim a lost Dwarf Kingdom from a dragon.", 10m, 2, "product-details-3.jpg", true, false, "product-3.jpg", 12.99m, 12, "The Hobbit", null },
                    { 4, 4, "SHINING", new DateTime(2024, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "A family heads to an isolated hotel for the winter where a sinister presence the father violence.", 0m, 2, "product-details-4.jpg", false, false, "product-4.jpg", 13.99m, 7, "The Shining", null },
                    { 5, 5, "MOTOE", new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Detective Hercule Poirot investigates a murder on a snowbound train.", 15m, 3, "product-details-5.jpg", true, true, "product-5.jpg", 14.99m, 9, "Murder on the Orient Express", null },
                    { 6, 1, "HP2", new DateTime(2024, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Harry returns for his second year at Hogwarts and faces new dangers.", 0m, 1, "product-details-1.jpg", false, false, "product-6.jpg", 15.99m, 10, "Harry Potter and the Chamber of Secrets", null },
                    { 7, 2, "GOT2", new DateTime(2024, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Seven Kingdoms are plagued by civil war as rival claimants to the Iron Throne emerge.", 5m, 1, "product-details-2.jpg", true, true, "product-7.jpg", 16.99m, 6, "A Clash of Kings", null },
                    { 8, 3, "LOTR1", new DateTime(2024, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Frodo Baggins sets out on a journey to destroy the One Ring.", 10m, 2, "product-details-3.jpg", false, false, "product-8.jpg", 17.99m, 11, "The Lord of the Rings: The Fellowship of the Ring", null },
                    { 9, 4, "CARRIE", new DateTime(2024, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "A bullied high school girl discovers her telekinetic powers with tragic results.", 0m, 2, "product-details-4.jpg", true, true, "product-9.jpg", 18.99m, 5, "Carrie", null },
                    { 10, 5, "ATTWN", new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ten strangers are invited to an island, where they are killed one by one.", 15m, 3, "product-details-5.jpg", false, false, "product-10.jpg", 19.99m, 8, "And Then There Were None", null }
                });

            migrationBuilder.InsertData(
                table: "BookImages",
                columns: new[] { "Id", "BookId", "ImageUrl" },
                values: new object[,]
                {
                    { 1, 1, "product-1.jpg" },
                    { 2, 2, "product-2.jpg" },
                    { 3, 3, "product-3.jpg" },
                    { 4, 4, "product-4.jpg" },
                    { 5, 5, "product-5.jpg" },
                    { 6, 6, "product-6.jpg" },
                    { 7, 7, "product-7.jpg" },
                    { 8, 8, "product-8.jpg" },
                    { 9, 9, "product-9.jpg" },
                    { 10, 10, "product-10.jpg" },
                    { 11, 1, "product-11.jpg" },
                    { 12, 2, "product-12.jpg" },
                    { 13, 3, "product-13.jpg" },
                    { 14, 4, "product-details-1.jpg" },
                    { 15, 5, "product-details-2.jpg" },
                    { 16, 6, "product-details-3.jpg" },
                    { 17, 7, "product-details-4.jpg" },
                    { 18, 8, "product-details-5.jpg" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookImages_BookId",
                table: "BookImages",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorId",
                table: "Books",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_GenreId",
                table: "Books",
                column: "GenreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookImages");

            migrationBuilder.DropTable(
                name: "Sliders");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Genres");
        }
    }
}

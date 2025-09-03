using Pustok.App.Models;

namespace Pustok.App.ViewModels
{
    public class BookDetailVM
    {
        public List<Book> RelatedBooks { get; set; }
        public Book Book { get; set; }
    }
}

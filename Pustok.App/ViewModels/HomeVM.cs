using Pustok.App.Models;

namespace Pustok.App.ViewModels
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }
        public List<Book> FeaturedBooks { get; set; }
        public List<Book> NewBooks { get; set; }
        public List<Book> DicountBooks { get; set; }
        public List<Featured> Featured { get; set; }
    }
}

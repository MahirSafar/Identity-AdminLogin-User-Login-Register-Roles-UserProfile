using Pustok.App.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace Pustok.App.Models
{
    public class BookImage : BaseEntity
    {
        [Required]
        public string ImageUrl { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}

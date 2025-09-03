using Pustok.App.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace Pustok.App.Models
{
    public class Genre : BaseEntity
    {
        [Required]
        [StringLength(20, ErrorMessage = "Name cannot be longer than 20 characters")]
        public string Name { get; set; }
        public List<Book> Books { get; set; }
    }
}

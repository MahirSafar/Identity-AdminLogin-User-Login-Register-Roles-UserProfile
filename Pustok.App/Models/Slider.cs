using Pustok.App.Attributes;
using Pustok.App.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pustok.App.Models
{
    public class Slider : AuditEntity
    {
        public string ImageUrl { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string ButtonText { get; set; }
        public string ButtonLink { get; set; }
        public int Order { get; set; }
        [NotMapped]
        [FileLength(2)]
        [FileType("image/jpeg","image/png", "image/jpg")]
        public IFormFile File { get; set; }
    }
}

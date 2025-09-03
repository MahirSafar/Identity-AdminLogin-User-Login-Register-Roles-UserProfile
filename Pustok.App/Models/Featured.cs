using Pustok.App.Models.Common;

namespace Pustok.App.Models
{
    public class Featured : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }
}

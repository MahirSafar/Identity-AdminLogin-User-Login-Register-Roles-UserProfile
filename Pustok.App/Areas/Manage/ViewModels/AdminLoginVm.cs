using System.ComponentModel.DataAnnotations;

namespace Pustok.App.Areas.Manage.ViewModels
{
    public class AdminLoginVm
    {
        [Required]
        public string Username { get; set; }
        [Required, MaxLength(10)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Pustok.App.ViewModels
{
    public class UserLoginVm
    {
        [Required]
        public string UsernameOrEmail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}

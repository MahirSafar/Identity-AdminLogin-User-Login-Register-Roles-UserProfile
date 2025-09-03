using System.ComponentModel.DataAnnotations;

namespace Pustok.App.ViewModels
{
    public class UserUpdateProfileVM
    {
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string NewPassword{ get; set; }
        [MinLength(6)]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "password not match")]
        public string ConfirmPassword { get; set; }
    }
}

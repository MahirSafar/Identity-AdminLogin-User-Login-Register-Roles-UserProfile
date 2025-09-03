using Microsoft.AspNetCore.Identity;

namespace Pustok.App
{
    public class CustomErrorDescriber:IdentityErrorDescriber
    {
        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError 
            {
                Code = "PasswordRequiresNonAlphanumeric",
                Description = "Parolda en az 1 simvol olmalidir" 
            };
        }
    }
}

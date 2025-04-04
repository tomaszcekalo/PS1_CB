using Microsoft.AspNetCore.Identity;

namespace PS1CB.Areas.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public int LockoutCount { get; set; } = 0;
    }
}

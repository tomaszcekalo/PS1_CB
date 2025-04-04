using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace PS1CB.Areas.Identity
{
    public class CustomSignInManager : SignInManager<ApplicationUser>
    {
        public CustomSignInManager(
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<ApplicationUser>> logger,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<ApplicationUser> confirmation)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation) { }

        public override async Task<SignInResult> CheckPasswordSignInAsync(ApplicationUser user, string password, bool lockoutOnFailure)
        {
            var result = await base.CheckPasswordSignInAsync(user, password, lockoutOnFailure);

            if (result == SignInResult.LockedOut)
                return result;

            if (result == SignInResult.Failed && lockoutOnFailure)
            {
                user.LockoutCount++;
                int lockoutMinutes = 5 * user.LockoutCount; // Increase by 5 mins for each lockout

                await UserManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddMinutes(lockoutMinutes));
                await UserManager.AccessFailedAsync(user); // Optional if you want to increment AccessFailedCount

                await UserManager.UpdateAsync(user);
            }

            return result;
        }
    }

}

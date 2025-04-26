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

            await LockoutFeature(user, result);

            return result;
        }

        public async Task LockoutFeature(ApplicationUser user, SignInResult result)
        {
            if (result == SignInResult.LockedOut)
                return;

            if (result == SignInResult.Failed )
            {
                user.LockoutCount++;
                int lockoutMinutes = 5 * user.LockoutCount; // Increase by 5 mins for each lockout

                await UserManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddMinutes(lockoutMinutes));
                await UserManager.AccessFailedAsync(user); // Optional if you want to increment AccessFailedCount

                await UserManager.UpdateAsync(user);
            }
        }
       
        public async Task<SignInResult> SignInFailedAsync(ApplicationUser user)
        {
            var incrementLockoutResult = await UserManager.AccessFailedAsync(user) ?? IdentityResult.Success;
            if (!incrementLockoutResult.Succeeded)
            {
                // Return the same failure we do when resetting the lockout fails after a correct password.
                await LockoutFeature(user, SignInResult.Failed);
                return SignInResult.Failed;
            }

            if (await UserManager.IsLockedOutAsync(user))
            {
                return await LockedOut(user);
            }
            return SignInResult.NotAllowed;
        }
    }

}

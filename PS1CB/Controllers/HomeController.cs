using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using PS1CB.Areas.Identity;
using PS1CB.Data;
using PS1CB.Models;
using PS1CB.Services;

namespace PS1CB.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly CustomSignInManager _signInManager;
    private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
    private readonly ILoginAttemptService _loginAttemptService;

    public HomeController(ILogger<HomeController> logger,
            UserManager<ApplicationUser> userManager,
            CustomSignInManager signInManager,
            IPasswordHasher<ApplicationUser> hasher,
            ILoginAttemptService loginAttemptService,
            ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
        _logger = logger;
        _signInManager = signInManager;
        _passwordHasher = hasher;
        _loginAttemptService = loginAttemptService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult LoginWithPartialPassword()
    {
        return View();
    }

    [HttpPost]
    public IActionResult LoginWithPartialPassword(LoginViewModel loginViewModel)
    {
        return RedirectToAction("SubPasswordLogin", "Home", new { Email=loginViewModel.Email,/* area = "Identity"*/ });
    }
    public async Task<IActionResult> SubPasswordLogin(LoginViewModel loginViewModel)
    {
        var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
        if (user == null) 
            return View("Login", model: null); // user not found

        var subs = await _context.SubPasswords
            .Where(x => x.UserId == user.Id)
            .OrderBy(x => x.Id)
            .ToListAsync();
        var sla = _loginAttemptService.GetSuccessfulLoginAttemptsCount(loginViewModel.Email);
        var sub = subs[sla % subs.Count];

        var vm = new SubPasswordLoginViewModel
        {
            Email = user.Email,
            UserId = user.Id,
            PositionString = sub.PositionString
        };

        return View("SubPasswordLogin", vm);
    }
    [HttpPost]
    public async Task<IActionResult> VerifyPartialPassword(SubPasswordLoginViewModel model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user == null) return RedirectToAction("LoginWithPartialPassword");
        if (await _userManager.IsLockedOutAsync(user))
        {
            _logger.LogWarning("User account locked out.");
            return RedirectToPage("./Lockout");
        }

        string input = string.Concat(model.Inputs);
        var sub = await _context.SubPasswords.FirstOrDefaultAsync(x =>
            x.UserId == user.Id && x.PositionString == model.PositionString);

        var result = _passwordHasher.VerifyHashedPassword(user, sub.Hash, input);
        if (result == PasswordVerificationResult.Success)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            await _loginAttemptService.AddLoginAttemptAsync(model.Email, DateTime.Now, true);
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", "Invalid password fragment.");
        await _loginAttemptService.AddLoginAttemptAsync(model.Email, DateTime.Now, false);
        await _signInManager.SignInFailedAsync(user);
        return View("SubPasswordLogin", model);

    }
}

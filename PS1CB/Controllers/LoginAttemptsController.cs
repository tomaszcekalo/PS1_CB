using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PS1CB.Data;

namespace PS1CB.Controllers
{
    public class LoginAttemptsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginAttemptsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LoginAttempts
        public async Task<IActionResult> Index()
        {
            return View(await _context.LoginAttempts.ToListAsync());
        }

    }
}

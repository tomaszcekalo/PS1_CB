using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PS1CB.Data;
using PS1CB.Models;

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
            var attempts = await _context.LoginAttempts
                .GroupBy(x => x.Who)
                .Select(g => new LoginAttemptModel
                {
                    Who = g.Key,
                    LastSuccessful = g.Where(x => x.Success).OrderByDescending(x => x.When).FirstOrDefault().When,
                    LastFailed = g.Where(x => !x.Success).OrderByDescending(x => x.When).FirstOrDefault().When,
                    //liczba nieudanych logowań od ostatniego poprawnego logowania
                    FailedCount = g.Where(x => !x.Success && x.When > g.Where(x => x.Success).OrderByDescending(x => x.When).FirstOrDefault().When).Count(),
                }).ToListAsync();
            return View(attempts);
        }

    }
}

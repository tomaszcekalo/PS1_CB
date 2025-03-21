using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PS1CB.Data;

namespace PS1CB.Controllers
{
    public class EditorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EditorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Editors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Editors.ToListAsync());
        }

        // GET: Editors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var editor = await _context.Editors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (editor == null)
            {
                return NotFound();
            }

            return View(editor);
        }

        // GET: Editors/Create
        public IActionResult Create(int id)
        {
            var message = _context.Messages.Find(id);
            if (message == null)
            {
                return NotFound();
            }
            if (message.AuthorName != User.Identity.Name)
            {
                return Unauthorized();
            }
            return View(new Editor()
            {
                MessageId = id
            });
        }

        // POST: Editors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MessageId,UserId")] Editor editor)
        {
            editor.Id = 0;
            if (ModelState.IsValid)
            {
                var message = await _context.Messages.FindAsync(editor.MessageId);
                if (message.AuthorName != User.Identity.Name)
                {
                    return Unauthorized();
                }
                _context.Add(editor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(editor);
        }

        // GET: Editors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var editor = await _context.Editors.FindAsync(id);
            if (editor == null)
            {
                return NotFound();
            }
            return View(editor);
        }

        // POST: Editors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MessageId,UserId")] Editor editor)
        {
            if (id != editor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(editor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EditorExists(editor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(editor);
        }

        // GET: Editors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var editor = await _context.Editors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (editor == null)
            {
                return NotFound();
            }

            return View(editor);
        }

        // POST: Editors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var editor = await _context.Editors.FindAsync(id);
            var message = _context.Messages.Find(id);
            if (message == null)
            {
                return NotFound();
            }
            if (message.AuthorName != User.Identity.Name)
            {
                return Unauthorized();
            }
            if (editor != null)
            {
                _context.Editors.Remove(editor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Messages", new { id=editor.MessageId} );
        }

        private bool EditorExists(int id)
        {
            return _context.Editors.Any(e => e.Id == id);
        }
    }
}

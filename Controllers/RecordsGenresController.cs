using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IJW2;
using IJW2.Models;

namespace IJW2.Controllers
{
    public class RecordsGenresController : Controller
    {
        private readonly WdtbContext _context;

        public RecordsGenresController(WdtbContext context)
        {
            _context = context;
        }

        // GET: RecordsGenres
        public async Task<IActionResult> Index()
        {
            var wdtbContext = _context.RecordsGenres.Include(r => r.Genre).Include(r => r.Record);
            return View(await wdtbContext.ToListAsync());
        }

        // GET: RecordsGenres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RecordsGenres == null)
            {
                return NotFound();
            }

            var recordsGenre = await _context.RecordsGenres
                .Include(r => r.Genre)
                .Include(r => r.Record)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recordsGenre == null)
            {
                return NotFound();
            }

            return View(recordsGenre);
        }

        // GET: RecordsGenres/Create
        public IActionResult Create()
        {
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Id");
            ViewData["RecordId"] = new SelectList(_context.Records, "Id", "Id");
            return View();
        }

        // POST: RecordsGenres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RecordId,GenreId")] RecordsGenre recordsGenre)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recordsGenre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Id", recordsGenre.GenreId);
            ViewData["RecordId"] = new SelectList(_context.Records, "Id", "Id", recordsGenre.RecordId);
            return View(recordsGenre);
        }

        // GET: RecordsGenres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RecordsGenres == null)
            {
                return NotFound();
            }

            var recordsGenre = await _context.RecordsGenres.FindAsync(id);
            if (recordsGenre == null)
            {
                return NotFound();
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Id", recordsGenre.GenreId);
            ViewData["RecordId"] = new SelectList(_context.Records, "Id", "Id", recordsGenre.RecordId);
            return View(recordsGenre);
        }

        // POST: RecordsGenres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RecordId,GenreId")] RecordsGenre recordsGenre)
        {
            if (id != recordsGenre.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recordsGenre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecordsGenreExists(recordsGenre.Id))
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
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Id", recordsGenre.GenreId);
            ViewData["RecordId"] = new SelectList(_context.Records, "Id", "Id", recordsGenre.RecordId);
            return View(recordsGenre);
        }

        // GET: RecordsGenres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RecordsGenres == null)
            {
                return NotFound();
            }

            var recordsGenre = await _context.RecordsGenres
                .Include(r => r.Genre)
                .Include(r => r.Record)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recordsGenre == null)
            {
                return NotFound();
            }

            return View(recordsGenre);
        }

        // POST: RecordsGenres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RecordsGenres == null)
            {
                return Problem("Entity set 'WdtbContext.RecordsGenres'  is null.");
            }
            var recordsGenre = await _context.RecordsGenres.FindAsync(id);
            if (recordsGenre != null)
            {
                _context.RecordsGenres.Remove(recordsGenre);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecordsGenreExists(int id)
        {
          return (_context.RecordsGenres?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

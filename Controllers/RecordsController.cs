using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IJW2.Models;
using static System.Net.Mime.MediaTypeNames;

namespace IJW2.Controllers
{
    public class RecordsController : Controller
    {
        private readonly WdtbContext _context;

        public RecordsController(WdtbContext context)
        {
            _context = context;
        }

        // GET: Records
        public async Task<IActionResult> Index(int? id, string? name)
        {
            /*return _context.Records != null ? 
                        View(await _context.Records.ToListAsync()) :
                        Problem("Entity set 'WdtbContext.Records'  is null.");*/

            if (id == null) return View(await _context.Records.ToListAsync());

            ViewBag.GenreId = id;
            ViewBag.GenreName = name;

            var RecordsGenresByGenre = _context.RecordsGenres.Where(rg => rg.GenreId == id).Include(rg => rg.Genre).ToList();
            List<int> RecordsList = new List<int>();
            foreach (var record in RecordsGenresByGenre)
            {
                RecordsList.Add(record.RecordId);
            }

            var RecordsByGenre = _context.Records.Where(r => RecordsList.Contains(r.Id));

            return View(await RecordsByGenre.ToListAsync());

        }

        // GET: Records/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Records == null)
            {
                return NotFound();
            }

            var @record = await _context.Records
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@record == null)
            {
                return NotFound();
            }

            return View(@record);
        }

        // GET: Records/Create
        public IActionResult Create(int GenreId)
        {
            //return View();
            ViewBag.GenreId = GenreId;
            ViewBag.GenreName = _context.Genres.Where(g => g.Id == GenreId).FirstOrDefault().Name;
            return View();
        }

        // POST: Records/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int GenreId, [Bind("Id,Name,ArtistId,Date,Quality,Information")] Record @record)
        {
            /*if (ModelState.IsValid)
            {
                _context.Add(@record);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@record);*/


            if (ModelState.IsValid)
            {
                _context.Add(@record);
                await _context.SaveChangesAsync();

                var rg = new RecordsGenre
                {
                    //Id = Guid.NewGuid().GetHashCode(),
                    RecordId = @record.Id,
                    GenreId = GenreId,
                };

                _context.RecordsGenres.Add(rg);
                _context.SaveChanges();
                return RedirectToAction("Index", "Records", new { id = GenreId, name = _context.Genres.Where(g => g.Id == GenreId).FirstOrDefault().Name});
            }

            return RedirectToAction("Index", "Records", new { id = GenreId, name = _context.Genres.Where(g => g.Id == GenreId).FirstOrDefault().Name });
        }

        // GET: Records/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Records == null)
            {
                return NotFound();
            }

            var @record = await _context.Records.FindAsync(id);
            if (@record == null)
            {
                return NotFound();
            }
            return View(@record);
        }

        // POST: Records/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ArtistId,Date,Quality,Information")] Record @record)
        {
            if (id != @record.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@record);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecordExists(@record.Id))
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
            return View(@record);
        }

        // GET: Records/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Records == null)
            {
                return NotFound();
            }

            var @record = await _context.Records
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@record == null)
            {
                return NotFound();
            }

            return View(@record);
        }

        // POST: Records/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Records == null)
            {
                return Problem("Entity set 'WdtbContext.Records'  is null.");
            }
            var @record = await _context.Records.FindAsync(id);
            if (@record != null)
            {
                _context.Records.Remove(@record);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecordExists(int id)
        {
          return (_context.Records?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

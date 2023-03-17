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
    public class RecordsArtistsController : Controller
    {
        private readonly WdtbContext _context;

        public RecordsArtistsController(WdtbContext context)
        {
            _context = context;
        }

        // GET: RecordsArtists
        public async Task<IActionResult> Index()
        {
            var wdtbContext = _context.RecordsArtists.Include(r => r.Artist).Include(r => r.Record);
            return View(await wdtbContext.ToListAsync());
        }

        // GET: RecordsArtists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RecordsArtists == null)
            {
                return NotFound();
            }

            var recordsArtist = await _context.RecordsArtists
                .Include(r => r.Artist)
                .Include(r => r.Record)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recordsArtist == null)
            {
                return NotFound();
            }

            return View(recordsArtist);
        }

        // GET: RecordsArtists/Create
        public IActionResult Create()
        {
            ViewData["ArtistId"] = new SelectList(_context.Artists, "Id", "Id");
            ViewData["RecordId"] = new SelectList(_context.Records, "Id", "Id");
            return View();
        }

        // POST: RecordsArtists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ArtistId,RecordId")] RecordsArtist recordsArtist)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recordsArtist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArtistId"] = new SelectList(_context.Artists, "Id", "Id", recordsArtist.ArtistId);
            ViewData["RecordId"] = new SelectList(_context.Records, "Id", "Id", recordsArtist.RecordId);
            return View(recordsArtist);
        }

        // GET: RecordsArtists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RecordsArtists == null)
            {
                return NotFound();
            }

            var recordsArtist = await _context.RecordsArtists.FindAsync(id);
            if (recordsArtist == null)
            {
                return NotFound();
            }
            ViewData["ArtistId"] = new SelectList(_context.Artists, "Id", "Id", recordsArtist.ArtistId);
            ViewData["RecordId"] = new SelectList(_context.Records, "Id", "Id", recordsArtist.RecordId);
            return View(recordsArtist);
        }

        // POST: RecordsArtists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ArtistId,RecordId")] RecordsArtist recordsArtist)
        {
            if (id != recordsArtist.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recordsArtist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecordsArtistExists(recordsArtist.Id))
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
            ViewData["ArtistId"] = new SelectList(_context.Artists, "Id", "Id", recordsArtist.ArtistId);
            ViewData["RecordId"] = new SelectList(_context.Records, "Id", "Id", recordsArtist.RecordId);
            return View(recordsArtist);
        }

        // GET: RecordsArtists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RecordsArtists == null)
            {
                return NotFound();
            }

            var recordsArtist = await _context.RecordsArtists
                .Include(r => r.Artist)
                .Include(r => r.Record)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recordsArtist == null)
            {
                return NotFound();
            }

            return View(recordsArtist);
        }

        // POST: RecordsArtists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RecordsArtists == null)
            {
                return Problem("Entity set 'WdtbContext.RecordsArtists'  is null.");
            }
            var recordsArtist = await _context.RecordsArtists.FindAsync(id);
            if (recordsArtist != null)
            {
                _context.RecordsArtists.Remove(recordsArtist);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecordsArtistExists(int id)
        {
          return (_context.RecordsArtists?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

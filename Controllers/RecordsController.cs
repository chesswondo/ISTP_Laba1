using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusBase.Models;
using static System.Net.Mime.MediaTypeNames;
using System.IO;

namespace MusBase.Controllers
{
    public class RecordsController : Controller
    {
        private readonly WdtbContext _context;

        public RecordsController(WdtbContext context)
        {
            _context = context;
        }

        // GET: Records
        public async Task<IActionResult> Index(int? some_id, string? name, string? source)
        {
            if (some_id == null) return View(await _context.Records.ToListAsync());


            if (source == "genre")
            {
                ViewBag.GenreId = some_id;
                ViewBag.GenreName = name;

                var RecordsGenresByGenre = _context.RecordsGenres.Where(rg => rg.GenreId == some_id).Include(rg => rg.Genre).ToList();
                List<int> RecordsList = new List<int>();
                foreach (var record in RecordsGenresByGenre)
                {
                    RecordsList.Add(record.RecordId);
                }

                var RecordsByGenre = _context.Records.Where(r => RecordsList.Contains(r.Id));

                return View(await RecordsByGenre.OrderBy(x => x.Name).ToListAsync());
            }

            else if (source == "artist")
            {
                ViewBag.ArtistId = some_id;
                ViewBag.ArtistName = name;

                var RecordsArtistsByArtist = _context.RecordsArtists.Where(ra => ra.ArtistId == some_id).Include(ra => ra.Artist).ToList();
                List<int> RecordsList = new List<int>();
                foreach (var record in RecordsArtistsByArtist)
                {
                    RecordsList.Add(record.RecordId);
                }

                var RecordsByArtist = _context.Records.Where(r => RecordsList.Contains(r.Id));

                return View(await RecordsByArtist.OrderBy(x => x.Name).ToListAsync());
            }

            
            else if (source == "label")
            {
                ViewBag.LabelId = some_id;
                ViewBag.LabelName = name;
                                
                var ArtistsByLabel = _context.Artists.Where(a => a.LabelId == some_id).ToList();
                List<int> ArtistIdList = new List<int>();
                foreach (var artist in ArtistsByLabel)
                {
                    ArtistIdList.Add(artist.Id);
                }

                List<int> RecordIdList = new List<int>();
                foreach(var rec_art in _context.RecordsArtists.ToList())
                {
                    if (ArtistIdList.Contains(rec_art.ArtistId)) RecordIdList.Add(rec_art.RecordId);
                }

                var RecordsByLabel = _context.Records.Where(r => RecordIdList.Contains(r.Id));
                return View(await RecordsByLabel.OrderBy(x => x.Name).ToListAsync());
            }
            
            
            else if (source == "country")
            {
                ViewBag.CountryId = some_id;
                ViewBag.CountryName = name;

                var ArtistsByCountry = _context.Artists.Where(a => a.CountryId == some_id).ToList();
                List<int> ArtistIdList = new List<int>();
                foreach (var artist in ArtistsByCountry)
                {
                    ArtistIdList.Add(artist.Id);
                }

                List<int> RecordIdList = new List<int>();
                foreach (var rec_art in _context.RecordsArtists.ToList())
                {
                    if (ArtistIdList.Contains(rec_art.ArtistId)) RecordIdList.Add(rec_art.RecordId);
                }

                var RecordsByCountry = _context.Records.Where(r => RecordIdList.Contains(r.Id));
                return View(await RecordsByCountry.OrderBy(x => x.Name).ToListAsync());
            }
            

            return RedirectToAction("Index", "Home");
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
        public IActionResult Create(int some_id, string source)
        {
            if (source == "genre")
            {
                ViewBag.GenreId = some_id;
                ViewBag.GenreName = _context.Genres.Where(g => g.Id == some_id).FirstOrDefault().Name;
            }

            else if (source == "artist")
            {
                ViewBag.ArtistId = some_id;
                ViewBag.ArtistName = _context.Artists.Where(a => a.Id == some_id).FirstOrDefault().Name;
            }

            else if (source == "label")
            {
                ViewBag.LabelId = some_id;
                ViewBag.LabelName = _context.Labels.Where(l => l.Id == some_id).FirstOrDefault().Name;
            }

            else if (source == "country")
            {
                ViewBag.CountryId = some_id;
                ViewBag.CountryName = _context.Countries.Where(c => c.Id == some_id).FirstOrDefault().Name;
            }

            return View();
        }

        // POST: Records/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int some_id, string source, [Bind("Id,Name,Date,Quality,Information")] Record @record)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@record);
                await _context.SaveChangesAsync();

                if (source == "genre")
                {
                    var rg = new RecordsGenre
                    {
                        RecordId = @record.Id,
                        GenreId = some_id,
                    };

                    _context.RecordsGenres.Add(rg);
                    _context.SaveChanges();
                    return RedirectToAction("Index", "Records", new { some_id = some_id, name = _context.Genres.Where(g => g.Id == some_id).FirstOrDefault().Name, source = "genre" });
                }


                else if (source == "artist")
                {
                    var ra = new RecordsArtist
                    {
                        RecordId = @record.Id,
                        ArtistId = some_id,
                    };

                    _context.RecordsArtists.Add(ra);
                    _context.SaveChanges();
                    return RedirectToAction("Index", "Records", new { some_id = some_id, name = _context.Artists.Where(a => a.Id == some_id).FirstOrDefault().Name, source = "artist" });
                }

                /*else if (source == "label")
                {
                    
                    
                    
                    
                    var ra = new RecordsArtist
                    {
                        RecordId = @record.Id,
                        ArtistId = some_id,
                    };

                    _context.RecordsArtists.Add(ra);
                    _context.SaveChanges();
                    return RedirectToAction("Index", "Records", new { some_id = some_id, name = _context.Artists.Where(a => a.Id == some_id).FirstOrDefault().Name, source = "artist" });
                }

                else if (source == "artist")
                {
                    var ra = new RecordsArtist
                    {
                        RecordId = @record.Id,
                        ArtistId = some_id,
                    };

                    _context.RecordsArtists.Add(ra);
                    _context.SaveChanges();
                    return RedirectToAction("Index", "Records", new { some_id = some_id, name = _context.Artists.Where(a => a.Id == some_id).FirstOrDefault().Name, source = "artist" });
                }
                */
            }

            return RedirectToAction("Index", "Home");
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
        
        public async Task<IActionResult> Delete(int id, int some_id, string source)
        {
            using (StreamWriter writer = new StreamWriter("text_id.txt", false))
            {
                await writer.WriteLineAsync(id.ToString());
            }

            using (StreamWriter writer = new StreamWriter("text_genre.txt", false))
            {
                await writer.WriteLineAsync(some_id.ToString());
            }

            using (StreamWriter writer = new StreamWriter("text_source.txt", false))
            {
                await writer.WriteLineAsync(source);
            }

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
        public async Task<IActionResult> DeleteConfirmed(int id, int some_id, string source)
        {
            int del_Id;
            int del_someId;
            string del_source;

            using (StreamReader reader = new StreamReader("text_id.txt"))
            {
                del_Id = Convert.ToInt32(await reader.ReadToEndAsync());
            }

            using (StreamReader reader = new StreamReader("text_genre.txt"))
            {
                del_someId = Convert.ToInt32(await reader.ReadToEndAsync());
            }

            using (StreamReader reader = new StreamReader("text_source.txt"))
            {
                del_source = await reader.ReadToEndAsync();
                del_source = del_source.Remove(del_source.Length - 2, 2);
            }

            if (_context.Records == null)
            {
                return Problem("Entity set 'WdtbContext.Records'  is null.");
            }
            var @record = await _context.Records.FindAsync(del_Id);


            if (del_source == "genre")
            {
                _context.RecordsGenres.RemoveRange(_context.RecordsGenres.Where(rg => rg.RecordId == del_Id));
                if (record != null) _context.Records.Remove(@record);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Records", new { some_id = del_someId, name = _context.Genres.Where(g => g.Id == del_someId).FirstOrDefault().Name, source = "genre" });
            }

            else if (del_source == "artist")
            {
                _context.RecordsArtists.RemoveRange(_context.RecordsArtists.Where(ra => ra.RecordId == del_Id));
                if (record != null) _context.Records.Remove(@record);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Records", new { some_id = del_someId, name = _context.Artists.Where(a => a.Id == del_someId).FirstOrDefault().Name, source = "artist" });
            }


            return RedirectToAction("Index", "Home");
        }

        private bool RecordExists(int id)
        {
          return (_context.Records?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

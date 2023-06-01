using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusBase;
using MusBase.Models;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IJW2.Controllers
{
    public class RequestsController : Controller
    {
        private readonly WdtbContext _context;

        public RequestsController(WdtbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Q1()
        {
            int n = 0;
            if (int.TryParse(Request.Form["n"], out int value)) n = Convert.ToInt32(Request.Form["n"]);
            if (n <= 0) return RedirectToAction(nameof(Index));

            QV.labels = new List<string> { "Id", "Name" };
            QV.results = new List<List<object>>();

            var result = from r in _context.Records
                         where (from ra in _context.RecordsArtists
                                where (from ra2 in _context.RecordsArtists
                                       group ra2 by ra2.ArtistId into g
                                       where g.Count() >= n
                                       select g.Key).Contains(ra.ArtistId)
                                select ra.RecordId).Distinct().Contains(r.Id)
                         select r;

            foreach (var i in result.ToList())
            { QV.results.Add(new List<object> { i.Id.ToString(), i.Name.ToString() }); }


            QV.SQLReq = "SELECT R.NAME\r\nFROM Records R\r\nWHERE R.Id IN\r\n\tSELECT DISTINCT RA.RecordId\r\n\tFROM RecordsArtists RA\r\n\tWHERE RA.ArtistId IN\r\n\t\tSELECT ArtistId\r\n\t\tFROM RecordsArtists\r\n\t\tGROUP BY ArtistId\r\n\t\tHAVING COUNT(*) >= n";
            QV.n = n;

            return RedirectToAction(nameof(Result));
        }


        public IActionResult Q2()
        {
            int n = 0;
            if (int.TryParse(Request.Form["n"], out int value)) n = Convert.ToInt32(Request.Form["n"]);
            if (n <= 0) return RedirectToAction(nameof(Index));

            QV.labels = new List<string> { "Id", "Name" };
            QV.results = new List<List<object>>();

            var result = from c in _context.Countries
                         where (from a in _context.Artists
                                where (from ra in _context.RecordsArtists
                                       group ra by ra.ArtistId into g
                                       where g.Count() == n
                                       select g.Key).Contains(a.Id)
                                select a.CountryId).Distinct().Contains(c.Id)
                         select c;

            foreach (var i in result.ToList())
            { QV.results.Add(new List<object> { i.Id.ToString(), i.Name.ToString() }); }

            QV.SQLReq = "SELECT Name\r\nFROM Countries\r\nWHERE Countries.Id IN\r\n\tSELECT DISTINCT CountryId\r\n\tFROM Artists\r\n\tWHERE Artists.Id IN\r\n\t\tSELECT ArtistId\r\n\t\tFROM RecordsArtists\r\n\t\tGROUP BY ArtistId\r\n\t\tHAVING COUNT(*) = n";
            QV.n = n;

            return RedirectToAction(nameof(Result));
        }


        public IActionResult Q3()
        {
            int n = 0;
            if (int.TryParse(Request.Form["n"], out int value)) n = Convert.ToInt32(Request.Form["n"]);
            if (n <= 0) return RedirectToAction(nameof(Index));

            QV.labels = new List<string> { "Id", "Name" };
            QV.results = new List<List<object>>();

            var result = from c in _context.Countries
                         where (from a in _context.Artists
                                group a by a.CountryId into g
                                where g.Count(x => x.LabelId != null) >= n
                                select g.Key).Contains(c.Id)
                         select c;

            foreach (var i in result.ToList())
            { QV.results.Add(new List<object> { i.Id.ToString(), i.Name.ToString() }); }

            QV.SQLReq = "SELECT Name\r\nFROM Countries C\r\nWHERE C.Id IN\r\n\tSELECT DISTINCT CountryId\r\n\tFROM Artists\r\n\tGROUP BY CountryId\r\n\tHAVING COUNT(LabelId) >= n";
            QV.n = n;

            return RedirectToAction(nameof(Result));
        }


        public IActionResult Q4()
        {
            int n = 0;
            if (int.TryParse(Request.Form["n"], out int value)) n = Convert.ToInt32(Request.Form["n"]);
            if (n <= 0) return RedirectToAction(nameof(Index));

            QV.labels = new List<string> { "Id", "Name" };
            QV.results = new List<List<object>>();

            var topGenres = _context.RecordsGenres
                .GroupBy(rg => rg.GenreId)
                .OrderByDescending(group => group.Count())
                .Take(n)
                .Select(group => group.Key)
                .ToList();

            var result = _context.Records
                .Where(r => _context.RecordsGenres
                    .Where(rg => topGenres.Contains(rg.GenreId))
                    .Select(rg => rg.RecordId)
                    .Contains(r.Id))
                .ToList();

            foreach (var i in result.ToList())
            { QV.results.Add(new List<object> { i.Id.ToString(), i.Name.ToString() }); }

            QV.SQLReq = "SELECT Name\r\nFROM Records\r\nWHERE Id IN (\r\n\tSELECT RecordId \r\n\tFROM RecordsGenres\r\n\tWHERE GenreId IN (SELECT TOP n GenreId\r\n  \t\tFROM RecordsGenres\r\n  \t\tGROUP BY GenreId\r\n  \t\tORDER BY COUNT(*) DESC));";
            QV.n = n;

            return RedirectToAction(nameof(Result));
        }


        public IActionResult Q5()
        {
            int n = 0;
            if (int.TryParse(Request.Form["n"], out int value)) n = Convert.ToInt32(Request.Form["n"]);
            if (n <= 0) return RedirectToAction(nameof(Index));

            QV.labels = new List<string> { "Id", "Name" };
            QV.results = new List<List<object>>();

            var result = _context.Countries
                .Where(country => _context.Artists
                    .GroupBy(artist => artist.CountryId)
                    .OrderByDescending(group => group.Count())
                    .Take(n)
                    .Select(group => group.Key)
                    .Contains(country.Id));

            foreach (var i in result.ToList())
            { QV.results.Add(new List<object> { i.Id.ToString(), i.Name.ToString() }); }

            QV.SQLReq = "SELECT Name\r\nFROM Countries\r\nWHERE Id IN (\r\n\tSELECT TOP n CountryId\r\n\tFROM Artists\r\n\tGROUP BY CountryId\r\n\tORDER BY COUNT(LabelId) DESC)";
            QV.n = n;

            return RedirectToAction(nameof(Result));
        }


        public IActionResult Q6()
        {
            QV.labels = new List<string> { "Name1", "Name2" };
            QV.results = new List<List<object>>();

            var result = from x in _context.Records
                         join y in _context.Records on x.Quality equals y.Quality into temp
                         from y in temp.DefaultIfEmpty()
                         where (x.Id != y.Id) && !(
                             (
                                 (from rgx in _context.RecordsGenres
                                  where rgx.RecordId == x.Id
                                  select rgx.GenreId).Distinct()
                                 .Except(
                                     from rgy in _context.RecordsGenres
                                     where rgy.RecordId == y.Id
                                     select rgy.GenreId
                                 )
                             )
                             .Union(
                                 (from rgy in _context.RecordsGenres
                                  where rgy.RecordId == y.Id
                                  select rgy.GenreId).Distinct()
                                 .Except(
                                     from rgx in _context.RecordsGenres
                                     where rgx.RecordId == x.Id
                                     select rgx.GenreId
                                 )
                             )
                         ).Any()
                         select new { x, y };

            foreach (var i in result.ToList())
            { QV.results.Add(new List<object> { i.x.Name.ToString(), i.y.Name.ToString() }); }


            QV.SQLReq = "SELECT X.Name, Y.Name\r\nFROM Records X INNER JOIN Records Y\r\nON (X.Quality = Y.Quality AND X.Id <> Y.Id)\r\nWHERE NOT EXISTS\r\n(\r\n\t\t((SELECT DISTINCT GenreId\r\n\t\tFROM RecordsGenres RGX\r\n\t\tWHERE RGX.RecordId = X.Id)\r\n\tEXCEPT\r\n\t\t(SELECT DISTINCT GenreId\r\n\t\tFROM RecordsGenres RGY\r\n\t\tWHERE RGY.RecordId = Y.Id))\r\nUNION ALL\r\n\t\t((SELECT DISTINCT GenreId\r\n\t\tFROM RecordsGenres RGY\r\n\t\tWHERE RGY.RecordId = Y.Id)\r\n\tEXCEPT\r\n\t\t(SELECT DISTINCT GenreId\r\n\t\tFROM RecordsGenres RGX\r\n\t\tWHERE RGX.RecordId = X.Id))\r\n)\r\n";
            QV.n = 0;

            return RedirectToAction(nameof(Result));
        }


        public IActionResult Q7()
        {
            QV.labels = new List<string> { "Name1", "Name2" };
            QV.results = new List<List<object>>();


            var result = from x in _context.Labels
                         join y in _context.Labels on true equals true into temp
                         from y in temp
                         where (x.Id != y.Id) && !(
                             (
                                 (from ax in _context.Artists
                                  where ax.LabelId == x.Id
                                  select ax.Id).Distinct()
                                 .Except(
                                     from ay in _context.Artists
                                     where ay.LabelId == y.Id
                                     select ay.Id).Distinct()
                             )
                             .Union(
                                 (from ay in _context.Artists
                                  where ay.LabelId == y.Id
                                  select ay.Id).Distinct()
                                 .Except(
                                     from ax in _context.Artists
                                     where ax.LabelId == x.Id
                                     select ax.Id).Distinct()
                             )
                         ).Any()
                         select new { x, y };


            QV.SQLReq = "SELECT X.Name, Y.Name\r\nFROM Labels X INNER JOIN Labels Y\r\nON (X.Id <> Y.Id)\r\nWHERE NOT EXISTS\r\n((\r\n\t\t(SELECT DISTINCT Id\r\n\t\tFROM Artists AX\r\n\t\tWHERE AX.LabelId = X.Id)\r\n\tEXCEPT\r\n\t\t(SELECT DISTINCT Id\r\n\t\tFROM Artists AY\r\n\t\tWHERE AY.LabelId = Y.Id))\r\nUNION ALL\r\n\t\t((SELECT DISTINCT Id\r\n\t\tFROM Artists AY\r\n\t\tWHERE AY.LabelId = Y.Id)\r\n\tEXCEPT\r\n\t\t(SELECT DISTINCT Id\r\n\t\tFROM Artists AX\r\n\t\tWHERE AX.LabelId = X.Id))\r\n)\r\n";
            QV.n = 0;

            return RedirectToAction(nameof(Result));
        }


        public IActionResult Q8()
        {
            QV.labels = new List<string> { "Name1", "Name2" };
            QV.results = new List<List<object>>();


            var result = from x in _context.Labels
                         join y in _context.Labels on true equals true into temp
                         from y in temp
                         where (x.Id != y.Id ) && !(
                             (
                                 (from a in _context.Artists
                                  where a.LabelId == x.Id
                                  select a.CountryId).Distinct()
                                 .Except(
                                     from a in _context.Artists
                                     where a.LabelId == y.Id
                                     select a.CountryId
                                 )
                             )
                             .Union(
                                 (from a in _context.Artists
                                  where a.LabelId == y.Id
                                  select a.CountryId).Distinct()
                                 .Except(
                                     from a in _context.Artists
                                     where a.LabelId == x.Id
                                     select a.CountryId
                                 )
                             )
                         ).Any()
                         select new { x, y };


            foreach (var i in result.ToList())
            { QV.results.Add(new List<object> { i.x.Name.ToString(), i.y.Name.ToString() }); }

            QV.SQLReq = "SELECT X.Name, Y.Name\r\nFROM Labels X INNER JOIN Labels Y\r\nON (X.Id <> Y.Id)\r\nWHERE NOT EXISTS\r\n((\r\n\t\t(SELECT DISTINCT CountryId\r\n\t\tFROM Artists\r\n\t\tWHERE LabelId = X.Id)\r\n\tEXCEPT\r\n\t\t(SELECT DISTINCT CountryId\r\n\t\tFROM Artists\r\n\t\tWHERE LabelId = Y.Id))\r\nUNION ALL\r\n\t\t((SELECT DISTINCT CountryId\r\n\t\tFROM Artists\r\n\t\tWHERE LabelId = Y.Id)\r\n\tEXCEPT\r\n\t\t(SELECT DISTINCT CountryId\r\n\t\tFROM Artists\r\n\t\tWHERE LabelId = X.Id))\r\n)\r\n";
            QV.n = 0;

            return RedirectToAction(nameof(Result));
        }


        public IActionResult Result()
        {
            ViewData["Labels"] = QV.labels;
            ViewData["Results"] = QV.results;
            ViewData["SQLReq"] = QV.SQLReq;
            ViewData["n"] = QV.n;
            ViewBag.Labels = QV.labels;
            ViewBag.Result = QV.results;
            ViewBag.SQLReq = QV.SQLReq;
            ViewBag.n = QV.n;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
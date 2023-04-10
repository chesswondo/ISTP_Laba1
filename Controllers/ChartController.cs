using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MusBase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly WdtbContext _context;

        public ChartController(WdtbContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData_GenRec")]

        public JsonResult JsonData_GenRec()
        {
            var genres = _context.Genres.ToList();
            List<object> catGenres = new List<object>();
            catGenres.Add(new[] { "Жанр", "Кількість альбомів"});
            foreach (var genre in genres)
            {
                List<int> RecordsList = new List<int>();
                foreach (var recordgenre in _context.RecordsGenres.ToList())
                {
                    if (recordgenre.GenreId == genre.Id) RecordsList.Add(recordgenre.RecordId);
                }

                catGenres.Add(new object[] {genre.Name, RecordsList.Count});
            }

            return new JsonResult(catGenres);
        }

        [HttpGet("JsonData_CountryRec")]
        public JsonResult JsonData_CountryRec()
        {
            var countries = _context.Countries.ToList();
            List<object> catCountries = new List<object>();
            catCountries.Add(new[] { "Країна", "Кількість альбомів" });
            
            foreach (var country in countries)
            {
                List<int> RecordsList = new List<int>();
                foreach (var artist in _context.Artists.ToList())
                {
                    if (artist.CountryId == country.Id)
                    {
                        foreach (var rec_art in _context.RecordsArtists.ToList())
                        {
                            if (rec_art.ArtistId == artist.Id) RecordsList.Add(rec_art.RecordId);
                        }
                    }
                }

                catCountries.Add(new object[] { country.Name, RecordsList.Count });
            }

            return new JsonResult(catCountries);
        }
    }
}

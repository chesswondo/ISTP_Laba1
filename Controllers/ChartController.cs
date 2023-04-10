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

        [HttpGet("JsonData")]

        public JsonResult JsonData()
        {
            var genres = _context.Genres.ToList();
            List<object> catGenres = new List<object>();
            catGenres.Add(new[] { "Жанр", "Кількість альбомів"});
            foreach (var genre in genres)
            {
                List<int> RecordsList = new List<int>();
                foreach (var recordgenre in _context.RecordsGenres)
                {
                    if (recordgenre.GenreId == genre.Id) RecordsList.Add(recordgenre.RecordId);
                }

                catGenres.Add(new object[] {genre.Name, RecordsList.Count});
            }

            return new JsonResult(catGenres);
        }

    }
}

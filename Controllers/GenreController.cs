using eBook_manager.Models;
using eBook_manager.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eBook_manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IEbookRepository _ebookRepository;

        public GenreController(IEbookRepository ebookRepository)
        {
            _ebookRepository = ebookRepository ?? throw new ArgumentNullException(nameof(ebookRepository));
        }

        [HttpGet("genre/{genre}")]
        public async Task<ActionResult<IEnumerable<EbookDTO>>> GetEbooksByGenre(string genre)
        {
            var ebooks = await _ebookRepository.GetEbooksByGenre(genre);

            if (ebooks == null || !ebooks.Any())
            {
                return NotFound();
            }

            return Ok(ebooks);
        }

    }
}

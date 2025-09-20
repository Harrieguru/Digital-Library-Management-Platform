using eBook_manager.Models;
using eBook_manager.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;

namespace eBook_manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EbookController : ControllerBase
    {
        private readonly IEbookRepository _ebookRepository;

        public EbookController(IEbookRepository ebookRepository)
        {
            _ebookRepository = ebookRepository ?? throw new ArgumentNullException(nameof(ebookRepository));
        }

        [HttpGet("{isbn}")]
        public async Task<ActionResult<EbookDTO>> GetEbookByISBN(string isbn)
        {
            var ebook = await _ebookRepository.GetEbookByISBN(isbn);

            if (ebook == null)
            {
                return NotFound();
            }

            return Ok(ebook);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EbookDTO>>> GetAllEbooks()
        {
            var ebooks = await _ebookRepository.GetAllEbooks();

            return Ok(ebooks);
        }

        [HttpPost]
        public async Task<ActionResult> AddEbook(EbookDTO ebook)
        {
            await _ebookRepository.AddEbook(ebook);

            return CreatedAtAction(nameof(GetEbookByISBN), new { isbn = ebook.ISBN }, ebook);
        }

        [HttpPut("{isbn}")]
        public async Task<ActionResult> UpdateEbook(string isbn, EbookDTO ebook)
        {
            if (isbn != ebook.ISBN)
            {
                return BadRequest();
            }

            await _ebookRepository.UpdateEbook(ebook);

            return NoContent();
        }


        [HttpPatch("{isbn}")]
        public async Task<ActionResult> PartialUpdateEbook(string isbn, JsonPatchDocument<EbookDTO> patchDocument)
        {
            var ebook = await _ebookRepository.GetEbookByISBN(isbn);

            if (ebook == null)
            {
                return NotFound();
            }

            // Apply the patch document to the ebook directly
            patchDocument.ApplyTo(ebook);

            // Update the ebook after applying the patch
            await _ebookRepository.UpdateEbook(ebook);

            return NoContent();
        }



        [HttpDelete("{isbn}")]
        public async Task<ActionResult> DeleteEbook(string isbn)
        {
            await _ebookRepository.DeleteEbook(isbn);

            return NoContent();
        }
    }
}

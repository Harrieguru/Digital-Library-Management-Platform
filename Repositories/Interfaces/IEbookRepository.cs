using eBook_manager.Models;
using Microsoft.AspNetCore.JsonPatch;


namespace eBook_manager.Repositories.Interfaces
{
    public interface IEbookRepository
    {
        Task <EbookDTO> GetEbookByISBN(string isbn);
        Task<IEnumerable<EbookDTO>> GetAllEbooks();
        Task AddEbook(EbookDTO ebook);
        Task UpdateEbook(EbookDTO ebook);
        Task PartialUpdateEbook(string isbn, JsonPatchDocument<EbookDTO> patchDocument);
        Task DeleteEbook(string isbn);

        Task<IEnumerable<EbookDTO>> GetEbooksByGenre(string genre);
    }
}

using eBook_manager.Models;

namespace eBook_manager.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDTO> GetUserByEmail(string email);
        Task<IEnumerable<UserDTO>> GetAllUsers();
        Task AddUser(UserDTO user);
        Task UpdateUser(string email,UserDTO user);
        Task DeleteUserByEmail(string email);
    }
}

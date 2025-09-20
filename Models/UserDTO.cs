using System.ComponentModel.DataAnnotations;

namespace eBook_manager.Models
{
    public class UserDTO
    {
        [Key]
        public string username { get; set; }

        public string email { get; set; }
        public string password { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace BLL.ModelsDTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<NotificationDTO> Notifications { get; set; }
        public ICollection<BookDTO> Books { get; set; }
        public byte[] Icon { get; set; }
    }
}
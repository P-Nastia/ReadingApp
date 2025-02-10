using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ModelsDTO
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public List<ChapterDTO> Chapters { get; set; }
        public ICollection<UserDTO> Users { get; set; }
        public string DisplayBook => $"{Author}, {Name}";
        public string CoverURL { get; set; }
    }
}

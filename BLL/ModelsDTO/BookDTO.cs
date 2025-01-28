using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ModelsDTO
{
    public class BookDTO
    {
        public int Id { get; set; }
        public byte[] PdfData { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public ICollection<ParagraphDTO> Paragraphs { get; set; }
        public ICollection<UserDTO> Users { get; set; }
    }
}

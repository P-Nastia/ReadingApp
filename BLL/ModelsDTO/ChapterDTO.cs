using DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ModelsDTO
{
    public class ChapterDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BookId { get; set; }
        public BookDTO Book { get; set; }
        public List<ParagraphDTO> Paragraphs { get; set; }
    }
}

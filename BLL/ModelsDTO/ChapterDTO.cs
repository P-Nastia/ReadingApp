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

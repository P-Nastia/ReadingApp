namespace BLL.ModelsDTO
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public List<ChapterDTO> Chapters { get; set; }
        public List<ParagraphDTO> Paragraphs { get; set; }
        public ICollection<UserDTO> Users { get; set; }
        public string DisplayBook => $"{Author}, {Name}";

        public string CoverURL { get; set; }
    }
}

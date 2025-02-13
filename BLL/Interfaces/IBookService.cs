using BLL.ModelsDTO;

namespace BLL.Interfaces
{
    public interface IBookService<T1, T2, T3> where T1 : class where T2 : class where T3 : class
    {
        void AddBook(T1 item);
        Task AddComment(T3 itemToAdd);
        Task DeleteComment(T3 item);
        T1 GetBook(int id);
        IEnumerable<T1> GetAll();
        T1 GetByNameAndAuthor(string name, string author);
        ParagraphDTO GetParagraph(int id);
    }

}

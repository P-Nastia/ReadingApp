using BLL.ModelsDTO;

namespace BLL.Interfaces
{
    public interface IUserService<T1, T2, T3> where T1 : class where T2 : class where T3 : class
    {
        Task RemoveNotification(T2 userEntity, T3 entity);
        Task AddNotification(T2 userEntity, T3 entity);
        Task RemoveBook(T2 userEntity, T1 entity);
        Task AddBook(T2 userEntity, T1 entity);
        T1 GetBook(T2 entity, int id);
        Task Add(T2 item);
        Task Remove(T2 item);
        IEnumerable<T2> GetAll();
        T2 GetById(int id);
        Task UpdateUser(T2 item);
        ChapterDTO LoadParagraphs(ChapterDTO chapter);
        T1 LoadChapters(T1 book);
        T2 FindSimiliar(string nickname, string password, string email,bool login);
    }
}

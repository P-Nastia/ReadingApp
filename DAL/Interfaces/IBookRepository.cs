namespace DAL.Interfaces
{
    public interface IBookRepository<T1,T2,T3> where T1 : class where T2 : class where T3 : class
    {
        void AddBook(T1 item);
        Task AddComment(T3 itemToAdd);
        Task DeleteComment(T3 item);
    }
}

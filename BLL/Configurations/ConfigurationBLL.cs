using DAL;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Configurations
{
    public class ConfigurationBLL
    {
        public static void ConfigureServiceCollection(ServiceCollection services)
        {
            services.AddTransient(typeof(IUserRepository<BookEntity, UserEntity, NotificationEntity>), typeof(UserRepository));
            services.AddTransient(typeof(IBookRepository<BookEntity, ParagraphEntity, UserCommentEntity>), typeof(BooksRepository));

            services.AddDbContext<AppDBContext>();
        }
    }
}

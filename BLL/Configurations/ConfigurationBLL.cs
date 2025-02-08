using BLL.Interfaces;
using BLL.ModelsDTO;
using BLL.Services;
using DAL;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

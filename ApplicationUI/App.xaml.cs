using ApplicationUI.ViewModels;
using BLL.Configurations;
using BLL.Interfaces;
using BLL.ModelsDTO;
using BLL.Services;
using DAL.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace ApplicationUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;
        public App()
        {
            var services = new ServiceCollection();
            ConfigurationService(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigurationService(ServiceCollection services)
        {
            services.AddTransient(typeof(IUserService<BookDTO, UserDTO, NotificationDTO>),typeof(UserService));
            services.AddTransient(typeof(IBookService<BookDTO, ParagraphDTO, UserCommentDTO>),typeof(BooksService));
            services.AddTransient(typeof(UserService));
            services.AddTransient(typeof(BooksService));
            services.AddTransient(typeof(SignupPageVM));
            services.AddTransient(typeof(LoginPageVM));
            services.AddTransient(typeof(MyLibraryPageVM));
            services.AddTransient(typeof(AllBooksPageVM));
            services.AddTransient(typeof(MyProfilePageVM));
            services.AddTransient(typeof(NotificationPageVM));
            services.AddTransient(typeof(PageViewModel));
            
            services.AddTransient(typeof(MainWindow));

            ConfigurationBLL.ConfigureServiceCollection(services);
        }
        private void OnStartUp(object sender, StartupEventArgs e)
        {
            var mainWind = _serviceProvider.GetService<MainWindow>();
            mainWind.Show();
        }
    }

}

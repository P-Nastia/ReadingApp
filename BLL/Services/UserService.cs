using AutoMapper;
using BLL.Interfaces;
using BLL.ModelsDTO;
using DAL.Entities;
using DAL.Interfaces;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Mapping;
using AutoMapper.QueryableExtensions;

namespace BLL.Services
{
    public class UserService : IUserService<BookDTO, UserDTO, NotificationDTO>
    {
        private readonly IUserRepository<BookEntity, UserEntity, NotificationEntity> _userRepository;
        private IMapper _mapper;
        public UserService(IUserRepository<BookEntity, UserEntity, NotificationEntity> repository)
        {
            _userRepository = repository;
            var configuration = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = configuration.CreateMapper();
        }
        public async Task Add(UserDTO item)
        {
            var user = _mapper.Map<UserDTO, UserEntity>(item);
            await _userRepository.Add(user);
        }

        public async Task AddBook(UserDTO userEntity, BookDTO entity)
        {
            var user = _mapper.Map<UserDTO, UserEntity>(userEntity);
            var book = _mapper.Map<BookDTO, BookEntity>(entity);
            await _userRepository.AddBook(user, book);
        }

        public async Task AddNotification(UserDTO userEntity, NotificationDTO entity)
        {
            var user = _mapper.Map<UserDTO, UserEntity>(userEntity);
            var notification = _mapper.Map<NotificationDTO, NotificationEntity>(entity);
            await _userRepository.AddNotification(user, notification);
        }

        public IEnumerable<UserDTO> GetAll()
        {
            AppDBContext context = new AppDBContext();
            return context.Users.AsQueryable().ProjectTo<UserDTO>(_mapper.ConfigurationProvider);
        }
        public BookDTO LoadParagraphs(BookDTO book)
        {
            AppDBContext context = new AppDBContext();
            if(book.Paragraphs == null)
            {
                book.Paragraphs = context.Paragraphs.AsQueryable()
               .Where(x => x.BookId == book.Id)
               .Skip(0)
               .Take(15)
               .ProjectTo<ParagraphDTO>(_mapper.ConfigurationProvider)
               .ToList();
                return book;
            }
            else
            {
                book.Paragraphs.AddRange(context.Paragraphs.AsQueryable()
                .Where(x => x.BookId == book.Id)
                .Skip(book.Paragraphs.Count)
                .Take(10)
                .ProjectTo<ParagraphDTO>(_mapper.ConfigurationProvider)
                .ToList());
                return book;
            }
        }

        public BookDTO GetBook(UserDTO entity, int id)
        {
            AppDBContext context = new AppDBContext();
            return context.Books.AsQueryable().Where(x => x.Id == id).ProjectTo<BookDTO>(_mapper.ConfigurationProvider).FirstOrDefault();
        }

        public UserDTO GetById(int id)
        {
            AppDBContext context = new AppDBContext();
            return context.Users.AsQueryable().Where(x => x.Id == id).ProjectTo<UserDTO>(_mapper.ConfigurationProvider).FirstOrDefault();
        }

        public async Task Remove(UserDTO item)
        {
            var user = _mapper.Map<UserDTO, UserEntity>(item);
            await _userRepository.Remove(user);
        }

        public async Task RemoveBook(UserDTO userEntity, BookDTO entity)
        {
            var user = _mapper.Map<UserDTO, UserEntity>(userEntity);
            var book = _mapper.Map<BookDTO, BookEntity>(entity);
            await _userRepository.RemoveBook(user, book);
        }

        public async Task RemoveNotification(UserDTO userEntity, NotificationDTO entity)
        {
            var user = _mapper.Map<UserDTO, UserEntity>(userEntity);
            var notification = _mapper.Map<NotificationDTO, NotificationEntity>(entity);
            await _userRepository.RemoveNotification(user, notification);
        }

        public async Task UpdateUser(UserDTO item)
        {
            var user = _mapper.Map<UserDTO, UserEntity>(item);
            await _userRepository.UpdateUser(user);
        }
    }
}

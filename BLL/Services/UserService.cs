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

namespace BLL.Services
{
    public class UserService : IUserService<BookDTO, UserDTO>
    {
        private readonly IUserRepository<BookEntity, UserEntity> _userRepository;
        private IMapper _mapper;
        public UserService(IUserRepository<BookEntity, UserEntity> repository)
        {
            //AppDBContext context = new AppDBContext();
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

        public IEnumerable<UserDTO> GetAll()
        {
            var list = new List<UserDTO>();
            foreach(var en in _userRepository.GetAll())
            {
                list.Add(_mapper.Map<UserEntity, UserDTO>(en));
            }
            return list;
        }

        public BookDTO GetBook(UserDTO entity, int id)
        {
            var user = _mapper.Map<UserDTO, UserEntity>(entity);
            var book = _userRepository.GetBook(user, id);
            return _mapper.Map<BookEntity, BookDTO>(book);
        }

        public UserDTO GetById(int id)
        {
            return _mapper.Map<UserEntity, UserDTO>(_userRepository.GetById(id));
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
    }
}

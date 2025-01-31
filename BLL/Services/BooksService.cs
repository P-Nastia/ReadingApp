using AutoMapper;
using BLL.Interfaces;
using BLL.Mapping;
using BLL.ModelsDTO;
using DAL;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class BooksService : IBookService<BookDTO,ParagraphDTO,UserCommentDTO>
    {
        private readonly IBookRepository<BookEntity, ParagraphEntity, UserCommentEntity> _bookRepository;
        private IMapper _mapper;
        public BooksService(IBookRepository<BookEntity, ParagraphEntity, UserCommentEntity> repository)
        {
            _bookRepository = repository;
            var configuration = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = configuration.CreateMapper();
        }

        public void AddBook(BookDTO item)
        {
            var book = _mapper.Map<BookDTO, BookEntity>(item);
            _bookRepository.AddBook(book);
        }

        public async Task AddComment(UserCommentDTO itemToAdd)
        {
            var comment = _mapper.Map<UserCommentDTO, UserCommentEntity>(itemToAdd);
            await _bookRepository.AddComment(comment);
        }

        public async Task DeleteComment(UserCommentDTO item)
        {
            var comment = _mapper.Map<UserCommentDTO, UserCommentEntity>(item);
            await _bookRepository.DeleteComment(comment);
        }

        public IEnumerable<BookDTO> GetAll()
        {
            var list = new List<BookDTO>();
            foreach (var en in _bookRepository.GetAll())
            {
                list.Add(_mapper.Map<BookEntity, BookDTO>(en));
            }
            return list;
        }

        public BookDTO GetBook(int id)
        {
            var book = _mapper.Map<BookEntity, BookDTO>(_bookRepository.GetBook(id));
            return book; 
        }

        public BookDTO GetByNameAndAuthor(string name, string author)
        {
            var book = _mapper.Map<BookEntity, BookDTO>(_bookRepository.GetByNameAndAuthor(name,author));
            return book;
        }
    }
}

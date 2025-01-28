using AutoMapper;
using BLL.Interfaces;
using BLL.Mapping;
using BLL.ModelsDTO;
using DAL;
using DAL.Entities;
using DAL.Interfaces;
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
            AppDBContext context = new AppDBContext();
            _bookRepository = repository;
            var configuration = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = configuration.CreateMapper();
        }

        public async Task AddBook(BookDTO item)
        {
            var book = _mapper.Map<BookDTO, BookEntity>(item);
            await _bookRepository.AddBook(book);
        }

        public async Task AddComment(ParagraphDTO itemTo, UserCommentDTO itemToAdd)
        {
            var paragraph = _mapper.Map<ParagraphDTO, ParagraphEntity>(itemTo);
            var comment = _mapper.Map<UserCommentDTO, UserCommentEntity>(itemToAdd);
            await _bookRepository.AddComment(paragraph, comment);
        }

        public async Task DeleteComment(UserCommentDTO item)
        {
            var comment = _mapper.Map<UserCommentDTO, UserCommentEntity>(item);
            await _bookRepository.DeleteComment(comment);
        }

        public BookDTO GetBook(int id)
        {
            var book = _mapper.Map<BookEntity, BookDTO>(_bookRepository.GetBook(id));
            return book; 
        }
    }
}

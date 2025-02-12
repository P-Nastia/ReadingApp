using AutoMapper;
using AutoMapper.QueryableExtensions;
using BLL.Interfaces;
using BLL.Mapping;
using BLL.ModelsDTO;
using DAL;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
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
            AppDBContext appDBContext = new AppDBContext();
            return appDBContext.Books.ProjectTo<BookDTO>(_mapper.ConfigurationProvider);
        }

        public ParagraphDTO GetParagraph(int id)
        {
            AppDBContext appDBContext = new AppDBContext();
            return appDBContext.Paragraphs.AsQueryable().Where(x => x.Id == id).ProjectTo<ParagraphDTO>(_mapper.ConfigurationProvider).FirstOrDefault();
        }
        public BookDTO GetBook(int id)
        {
            AppDBContext dBContext = new AppDBContext();
            return dBContext.Books.AsQueryable().Where(b => b.Id == id).ProjectTo<BookDTO>(_mapper.ConfigurationProvider).FirstOrDefault();
           
        }

        public BookDTO GetByNameAndAuthor(string name, string author)
        {
            AppDBContext dBContext = new AppDBContext();
            return dBContext.Books.AsQueryable().Where(b=>b.Author==author && b.Name==name).ProjectTo<BookDTO>(_mapper.ConfigurationProvider).FirstOrDefault();
        }
    }
}

﻿using AutoMapper;
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
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class UserService : IUserService<BookDTO, UserDTO>
    {
        private readonly IUserRepository<BookEntity, UserEntity> _userRepository;
        private IMapper _mapper;

        public UserService(IUserRepository<BookEntity, UserEntity> repository)
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


        public IEnumerable<UserDTO> GetAll()
        {
            AppDBContext context = new AppDBContext();
            return context.Users.AsQueryable().ProjectTo<UserDTO>(_mapper.ConfigurationProvider);
        }
        public ChapterDTO LoadParagraphs(ChapterDTO chapter)
        {
            AppDBContext context = new AppDBContext();
            if(chapter.Paragraphs == null)
            {
                chapter.Paragraphs = new List<ParagraphDTO>();
                chapter.Paragraphs = context.Paragraphs.AsQueryable().Where(x => x.ChapterId == chapter.Id).Skip(0).Take(15).ProjectTo<ParagraphDTO>(_mapper.ConfigurationProvider).ToList();
                return chapter;
            }
            else
            {
                chapter.Paragraphs.AddRange(context.Paragraphs.AsQueryable().Where(x => x.ChapterId == chapter.Id).Skip(chapter.Paragraphs.Count).Take(10).ProjectTo<ParagraphDTO>(_mapper.ConfigurationProvider).ToList());
                return chapter;
            }
        }
        public BookDTO LoadChapters(BookDTO book)
        {
            AppDBContext context = new AppDBContext();
            book.Chapters = context.Chapters.AsQueryable().Where(x => x.BookId == book.Id).ProjectTo<ChapterDTO>(_mapper.ConfigurationProvider).ToList();
            return book;
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

        public async Task UpdateUser(UserDTO item)
        {
            var user = _mapper.Map<UserDTO, UserEntity>(item);
            await _userRepository.UpdateUser(user);
        }
    }
}

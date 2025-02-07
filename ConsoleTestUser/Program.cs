using System.Diagnostics;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BLL.Mapping;
using BLL.ModelsDTO;
using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConsoleTestUser
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AppDBContext appDBContext = new AppDBContext();
            appDBContext.Users.Any();



            //------User------

            IMapper _mapper;

            var configuration = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = configuration.CreateMapper();


            var query = appDBContext.Users.AsQueryable();
            var itemQuery = query
                .ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
                .AsQueryable();

            var user = itemQuery.SingleOrDefault(x => x.Id == 1);

            //Skip().Take()


            user.Books = appDBContext.Books.AsQueryable()
                .Where(x => x.Users.Any(x => x.Id == user.Id))
                .ProjectTo<BookDTO>(_mapper.ConfigurationProvider)
                .ToList();


            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            user.Books[0].Paragraphs = appDBContext.Paragraphs.AsQueryable()
                .Where(x => x.BookId==1)
                .Skip(0)
                .Take(10)
                .ProjectTo<ParagraphDTO>(_mapper.ConfigurationProvider)
                .ToList();

            user.Books[0].Paragraphs.AddRange(appDBContext.Paragraphs.AsQueryable()
                .Where(x => x.BookId == 1)
                .Skip(user.Books[0].Paragraphs.Count)
                .Take(10)
                .ProjectTo<ParagraphDTO>(_mapper.ConfigurationProvider)
                .ToList());

            //-----Paragraphs-------
            //var query = appDBContext.Paragraphs.AsQueryable();
            //var list = query.ToList();

            //Console.WriteLine("Test database :)");

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
        }
    }
}

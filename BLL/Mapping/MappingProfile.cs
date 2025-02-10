using AutoMapper;
using BLL.ModelsDTO;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            EntitiesToDTOs();
            DTOsToEntities();
        }

        private void DTOsToEntities()
        {
            CreateMap<BookDTO, BookEntity>()
                .ForMember(x => x.Users, opt => opt.MapFrom(x => x.Users))
                .ForMember(x => x.Chapters, opt => opt.MapFrom(x => x.Chapters));

            CreateMap<ChapterDTO, ChapterEntity>()
                .ForMember(x => x.Paragraphs, opt => opt.MapFrom(x => x.Paragraphs))
                .ForMember(x => x.BookId, opt => opt.MapFrom(x => x.BookId))
                .ForMember(x => x.Book, opt => opt.MapFrom(x => x.Book))
                .ForMember(x => x.Id, opt => opt.Ignore()); ;

            CreateMap<ParagraphDTO, ParagraphEntity>()
                .ForMember(x => x.UserComments, opt => opt.MapFrom(x => x.UserComments))
                .ForMember(x => x.ChapterId, opt => opt.MapFrom(x => x.ChapterId))
                .ForMember(x => x.Chapter, opt => opt.MapFrom(x => x.Chapter))
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<UserCommentDTO, UserCommentEntity>()
                .ForMember(x => x.UserId, opt => opt.MapFrom(x => x.UserId))
                .ForMember(x => x.User, opt => opt.Ignore())
                .ForMember(x => x.ParagraphId, opt => opt.MapFrom(x => x.ParagraphId))
                .ForMember(x => x.Paragraph, opt => opt.Ignore());

            CreateMap<NotificationDTO, NotificationEntity>()
                .ForMember(x => x.HotLoadLink, opt => opt.MapFrom(x => x.HotLoadLink))
                .ForMember(x => x.Message, opt => opt.MapFrom(x => x.Message))
                .ForMember(x => x.Subject, opt => opt.MapFrom(x => x.Subject))
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<UserDTO, UserEntity>()
                .ForMember(x => x.Books, opt => opt.Ignore())
                .ForMember(x => x.Notifications, opt => opt.MapFrom(x=>x.Notifications));
        }

        private void EntitiesToDTOs()
        {
            CreateMap<BookEntity, BookDTO>()
                //.ForMember(x => x.Users, opt => opt.MapFrom(x => x.Users))
                .ForMember(x => x.Users, opt => opt.Ignore())
                //.ForMember(x => x.Paragraphs, opt => opt.MapFrom(x => x.Paragraphs))
                .ForMember(x => x.Chapters, opt => opt.Ignore())
                .ForMember(x => x.DisplayBook, opt => opt.Ignore());

            CreateMap<ChapterEntity, ChapterDTO>()
                .ForMember(x => x.Paragraphs, opt => opt.Ignore())
                .ForMember(x => x.BookId, opt => opt.MapFrom(x => x.BookId))
                .ForMember(x => x.Book, opt => opt.MapFrom(x => x.Book));

            CreateMap<ParagraphEntity, ParagraphDTO>()
                .ForMember(x => x.UserComments, opt => opt.MapFrom(x => x.UserComments))
                .ForMember(x => x.ChapterId, opt => opt.MapFrom(x => x.ChapterId))
                .ForMember(x => x.Chapter, opt => opt.MapFrom(x => x.Chapter))
                .ForMember(x => x.DisplayText, opt => opt.Ignore());

            CreateMap<UserCommentEntity, UserCommentDTO>()
                .ForMember(x => x.UserId, opt => opt.MapFrom(x => x.UserId))
                .ForMember(x => x.User, opt => opt.MapFrom(x => x.User))
                .ForMember(x => x.ParagraphId, opt => opt.MapFrom(x => x.ParagraphId))
                .ForMember(x => x.Paragraph, opt => opt.MapFrom(x => x.Paragraph));

            CreateMap<NotificationEntity, NotificationDTO>()
                .ForMember(x => x.HotLoadLink, opt => opt.MapFrom(x => x.HotLoadLink))
                .ForMember(x => x.Message, opt => opt.MapFrom(x => x.Message))
                .ForMember(x => x.Subject, opt => opt.MapFrom(x => x.Subject));

            CreateMap<UserEntity, UserDTO>()
                .ForMember(x => x.Books, opt => opt.MapFrom(x => x.Books))
                .ForMember(x => x.Notifications, opt => opt.MapFrom(x => x.Notifications));
        }
    }
}

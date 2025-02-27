using AutoMapper;
using Notes.DataLayer.Models;
using Notes.Main.Common;
using Notes.Main.Models.Note;
using Notes.Main.Models.User;

namespace Notes.Main.Mapper
{
    internal class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            CreateMap<CreateUserModel, User>()
                .ForMember(u => u.Name, m => m.MapFrom(s => s.UserName))
                .ForMember(u => u.Id, m => m.MapFrom(s => Guid.NewGuid()))
                .ForMember(u => u.PasswordHashed, m => m.MapFrom(s => HashMaker.GetSHA256Hash(s.Password)))
                .ForMember(u => u.CreateDate, m => m.MapFrom(s => DateTime.Now));
            CreateMap<CreateNoteModel, Note>()
                .ForMember(u => u.Id, m => m.MapFrom(s => Guid.NewGuid()))
                .ForMember(u => u.CreateDate, m => m.MapFrom(s => DateTime.Now));
            CreateMap<Note, GetNoteModel>()
                .ForMember(g => g.AuthorName, m => m.MapFrom(n => n.Author.Name));
        }
    }
}

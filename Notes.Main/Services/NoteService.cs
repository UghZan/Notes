using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Notes.DataLayer;
using Notes.DataLayer.Models;
using Notes.Main.Models.Note;

namespace Notes.Main.Services
{
    public class NoteService
    {
        private readonly DataContext context;
        private readonly IMapper mapper;
        private readonly UserService userService;
    
        public NoteService(DataContext _context, IMapper _mapper, UserService _userService)
        {
            this.context = _context;
            this.mapper = _mapper;
            this.userService = _userService;
        }

        internal async Task<IEnumerable<GetNoteModel>?> GetUserNotesAsync(Guid authorID)
        {
            var notes = await context.Notes.Where(n => n.AuthorId == authorID).Include(n => n.Author).Select(n => mapper.Map<GetNoteModel>(n)).ToListAsync();
            return notes;
        }

        internal async Task<IEnumerable<GetNoteModel>?> GetNotesAsync(bool publicOnly = true, int amount = 10)
        {
            var notes = await context.Notes.Where(n => publicOnly ? n.IsPublic : (n.IsPublic || !n.IsPublic)).Include(n => n.Author).Select(n => mapper.Map<GetNoteModel>(n)).Take(amount).ToListAsync();
            return notes;
        }

        internal async Task<Note> CreateNoteAsync(Guid authorID, CreateNoteModel model)
        {
            var newNote = mapper.Map<Note>(model);
            var author = await userService.GetUserByIDAsync(authorID);
            newNote.Author = author;
            var addedNote = await context.Notes.AddAsync(newNote);
            await context.SaveChangesAsync();
            return addedNote.Entity;
        }

        internal async Task EditNoteAsync(GetNoteModel model)
        {
            var existingNote = await context.Notes.FirstOrDefaultAsync(m => m.Id == model.Id);
            if (existingNote is null)
            {
                throw new Exception("Заметка не найдена");
            }
            existingNote.Name = model.Name;
            existingNote.LastEditDate = DateTimeOffset.Now;
            existingNote.Content = model.Content;
            existingNote.IsPublic = model.IsPublic;
            await context.SaveChangesAsync();
        }

        internal async Task<GetNoteModel> GetNoteById(string id)
        {
            if(Guid.TryParse(id, out Guid noteID))
            {
                var note = await context.Notes.Include(n => n.Author).FirstOrDefaultAsync(n => n.Id == noteID);
                if (note == null)
                {
                    throw new Exception("Заметка не найдена");
                }
                var noteModel = mapper.Map<GetNoteModel>(note);
                return noteModel;
            }
            else
            {
                throw new Exception("Заметка не найдена");
            }
        }
    }
}

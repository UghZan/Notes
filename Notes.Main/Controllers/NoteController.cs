using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notes.Main.Filters;
using Notes.Main.Models.Note;
using Notes.Main.Services;

namespace Notes.Main.Controllers
{
    [Route("Note")]
    [AuthState]
    public class NoteController : Controller
    {
        readonly NoteService noteService;

        public NoteController(NoteService _noteService) {
            this.noteService = _noteService;
        }

        [Route("viewnote")]
        public async Task<IActionResult> ViewNote(string id)
        {
            var note = await noteService.GetNoteById(id);
            return View(note);
        }

        [Route("editnote")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditNote(string id)
        {
            var note = await noteService.GetNoteById(id);
            return View(note);
        }
        [Route("editnote")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditNote(GetNoteModel model)
        {
            await noteService.EditNoteAsync(model);
            return RedirectToAction("Main", "User");
        }
    }
}

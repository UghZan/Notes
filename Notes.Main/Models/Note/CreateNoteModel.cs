namespace Notes.Main.Models.Note
{
    public class CreateNoteModel
    {
        public string? Name { get; set; }
        public string? Content { get; set; }
        public bool IsPublic { get; set; }
    }
}

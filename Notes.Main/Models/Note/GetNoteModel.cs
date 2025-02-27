namespace Notes.Main.Models.Note
{
    public class GetNoteModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Content { get; set; }
        public bool IsPublic { get; set; }

        public DateTimeOffset CreateDate { get; set; }

        public string AuthorName { get; set; }
    }
}

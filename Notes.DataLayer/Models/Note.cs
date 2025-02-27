using Microsoft.EntityFrameworkCore;

namespace Notes.DataLayer.Models
{
    public class Note
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Content { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset? LastEditDate { get; set; }
        public bool IsPublic { get; set; }

        public User Author { get; set; } = null!;
        public Guid AuthorId { get; set; }
    }
}

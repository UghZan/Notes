using Microsoft.Extensions.Hosting;

namespace Notes.DataLayer.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHashed { get; set; } = null!;
        public DateTimeOffset CreateDate { get; set; }
        public ICollection<Note> Notes { get; }
    }
}

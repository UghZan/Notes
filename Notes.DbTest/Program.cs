using Notes.DataLayer;
using System.Security.Policy;

namespace Notes.DbTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new DataContext())
            {
                context.Database.EnsureCreated();
            }
        }
    }
}

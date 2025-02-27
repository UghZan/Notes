using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Notes.DataLayer;
using Notes.DataLayer.Models;
using Notes.Main.Models.User;

namespace Notes.Main.Services
{
    public class UserService
    {
        private readonly DataContext context;
        private readonly IMapper mapper;

        public UserService(DataContext _context, IMapper _mapper)
        {
            context = _context;
            mapper = _mapper;
        }

        internal async Task<User> CreateNewUserAsync(CreateUserModel cum)
        {
            var newUser = mapper.Map<User>(cum);
            var addedUser = await context.Users.AddAsync(newUser);
            await context.SaveChangesAsync();
            return addedUser.Entity;
        }

        internal async Task<User> GetUserByCredentialsAsync(string login, string passwordHashed)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Name == login && u.PasswordHashed == passwordHashed);
            if(user == null)
            {
                throw new Exception("Пользователь с таким именем/паролем не найден");
            }
            return user;
        }
        internal async Task<User?> GetUserByNameAsync(string login)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Name == login);
            return user;
        }
        internal async Task<User> GetUserByIDAsync(Guid id)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new Exception("Пользователь с таким id не найден");
            }
            return user;
        }
    }
}

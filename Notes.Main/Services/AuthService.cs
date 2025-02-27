using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Notes.DataLayer;
using Notes.DataLayer.Models;
using System.Security.Claims;

namespace Notes.Main.Services
{
    public class AuthService
    {
        private readonly DataContext context;
        private readonly IMapper mapper;

        public AuthService(DataContext _context, IMapper _mapper)
        {
            context = _context;
            mapper = _mapper;
        }

        internal ClaimsPrincipal CreateUserClaims(User user)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Name), new Claim("userID", user.Id.ToString())};
            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            return new ClaimsPrincipal(identity);
        }

        internal Guid GetCurrentUserId(HttpContext currentContext)
        {
            return Guid.Parse(currentContext.User.Claims.FirstOrDefault(c => c.Type == "userID").Value);
        }
    }
}

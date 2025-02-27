using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Notes.DataLayer.Models;
using Notes.Main.Common;
using Notes.Main.Filters;
using Notes.Main.Models.User;
using Notes.Main.Services;
namespace Notes.Controllers
{
    [Route("Home")]
    [Route("")]
    [AuthState]
    public class HomeController : Controller
    {
        readonly DataLayer.DataContext dbContext;
        readonly UserService userService;
        readonly AuthService authService;
        readonly NoteService noteService;
        public HomeController(DataLayer.DataContext _context, UserService _userService, AuthService _authService, NoteService _noteService)
        {
            dbContext = _context;
            userService = _userService;
            authService = _authService;
            noteService = _noteService;
        }
        [Route("{controller=Home}/{action=Index}")]
        public async Task<IActionResult> Index()
        {
            var notes = await noteService.GetNotesAsync();
            return View(notes);
        }
        [Route("register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register(CreateUserModel user)
        {

           if (ModelState.IsValid)
           {
               var newUser = await userService.CreateNewUserAsync(user);
               var principal = authService.CreateUserClaims(newUser);
               await HttpContext.SignInAsync(principal);
               return Redirect("~/User/main");
            }
           else
           {
               return View();
           }

        }
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [Route("allnotes")]
        public IActionResult AllNotes()
        {
            return View();
        }

        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Index));
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginUserModel model)
        {
            if (ModelState.IsValid)
            {
                var login = model.UserName;
                var password = model.Password;
                var passwordHashed = HashMaker.GetSHA256Hash(model.Password);
                User possibleUser;
                try
                {
                    possibleUser = await userService.GetUserByCredentialsAsync(login, passwordHashed);
                }
                catch (Exception ex)
                {
                    this.ViewData["LoginError"] = ex.Message;
                    return View();
                }
                var principal = authService.CreateUserClaims(possibleUser);
                await HttpContext.SignInAsync(principal);
                return Redirect("~/User/main");
            }
            else
            {
                return View();
            }
        }
    }
}

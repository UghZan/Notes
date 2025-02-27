using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notes.DataLayer;
using Notes.Main.Common;
using Notes.Main.Filters;
using Notes.Main.Models.Note;
using Notes.Main.Models.User;
using Notes.Main.Services;

namespace Notes.Main.Controllers
{
    [Route("User")]
    [AuthState]
    public class UserController : Controller
    {
        readonly DataContext dbContext;
        readonly UserService userService;
        readonly NoteService noteService;
        readonly AuthService authService;

        public UserController(DataContext _dbContext, UserService _userService, NoteService _noteService, AuthService _authService)
        {
            dbContext = _dbContext;
            userService = _userService;
            noteService = _noteService;
            authService = _authService;
        }
        [Route("main")]
        [Authorize]
        public async Task<IActionResult> Main()
        {
            var userId = authService.GetCurrentUserId(HttpContext);
            var notesByUser = await noteService.GetUserNotesAsync(userId);
            return View(notesByUser);
        }
        [Route("addnote")]
        [HttpGet]
        [Authorize]
        public IActionResult AddNote()
        {
            return View();
        }
        [Route("addnote")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNote(CreateNoteModel noteModel)
        {
            if (ModelState.IsValid)
            {
                var newNote = await noteService.CreateNoteAsync(authService.GetCurrentUserId(HttpContext), noteModel);
                return RedirectToAction(nameof(Main));
            }
            else
            {
                return View();
            }
        }

        [Route("settings")]
        [Authorize]
        public async Task<IActionResult> Settings()
        {
            return View();
        }

        [Route("changeusername")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangeUsername(ChangeUserNameModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = authService.GetCurrentUserId(HttpContext);
                var passHashed = HashMaker.GetSHA256Hash(model.Password);
                var user = await userService.GetUserByIDAsync(userId);
                if (user != null)
                {
                    if (passHashed != user.PasswordHashed)
                    {
                        TempData["ChangeNameError"] = "Пароли не совпадают\n";
                        return RedirectToAction(nameof(Settings));
                    }
                    if (await userService.GetUserByNameAsync(model.NewUserName) != null)
                    {
                        TempData["ChangeNameError"] = "Пользователь с таким именем уже существует\n";
                        return RedirectToAction(nameof(Settings));
                    }
                    user.Name = model.NewUserName;
                    await dbContext.SaveChangesAsync();

                    await HttpContext.SignOutAsync();

                    var principal = authService.CreateUserClaims(user);
                    await HttpContext.SignInAsync(principal);

                    TempData["ChangeMessage"] = "Имя успешно сменено!";
                    return RedirectToAction(nameof(Settings));
                }
                TempData["ChangeNameError"] = "Внутренняя ошибка, повторите попытку\n";
                return RedirectToAction(nameof(Settings));
            }
            else
            {
                return RedirectToAction(nameof(Settings));
            }
        }

        [Route("changepassword")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = authService.GetCurrentUserId(HttpContext);
                var passHashed = HashMaker.GetSHA256Hash(model.Password);
                var user = await userService.GetUserByIDAsync(userId);
                if (user != null)
                {
                    if (passHashed != user.PasswordHashed)
                    {
                        TempData["ChangePassError"] = "Неверный текущий пароль";
                        return RedirectToAction(nameof(Settings));
                    }

                    var newPass = HashMaker.GetSHA256Hash(model.NewPassword);
                    user.PasswordHashed = newPass;
                    await dbContext.SaveChangesAsync();

                    await HttpContext.SignOutAsync();

                    var principal = authService.CreateUserClaims(user);
                    await HttpContext.SignInAsync(principal);

                    TempData["ChangeMessage"] = "Пароль успешно сменен!";
                    return RedirectToAction(nameof(Settings));
                }
                TempData["ChangePassError"] = "Внутренняя ошибка, повторите попытку";
                return RedirectToAction(nameof(Settings));
            }
            else
            {
                return RedirectToAction(nameof(Settings));
            }
        }
    }
}

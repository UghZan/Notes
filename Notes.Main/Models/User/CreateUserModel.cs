using System.ComponentModel.DataAnnotations;

namespace Notes.Main.Models.User
{
    public class CreateUserModel
    {
        [Required(ErrorMessage = "Введите имя пользователя")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Пароли не совпадают")]
        [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
        public string PasswordRepeat { get; set; }
        [Required(ErrorMessage = "Введите мыло")]
        public string Email { get; set; }
        public CreateUserModel() { }
        public CreateUserModel(string userName, string password, string passwordRepeat, string email)
        {
            UserName = userName;
            Password = password;
            PasswordRepeat = passwordRepeat;
            Email = email;
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Notes.Main.Models.User
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Введите текущий пароль")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Пароли не совпадают")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Пароли не совпадают")]
        [Compare(nameof(NewPassword), ErrorMessage = "Пароли не совпадают")]
        public string NewPasswordRepeat { get; set; }
    }
}

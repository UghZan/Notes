using System.ComponentModel.DataAnnotations;

namespace Notes.Main.Models.User
{
    public class ChangeUserNameModel
    {
        [Required(ErrorMessage = "Новое имя не может быть пустым")]
        public string NewUserName { get; set; }
        [Required(ErrorMessage = "Введите текущий пароль")]
        public string Password { get; set; }
    }
}

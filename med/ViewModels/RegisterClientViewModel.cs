using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace med.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [DisplayName("Логин")]

        public string Username { get; set; }
        [DisplayName("Пароль")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтвердите пароль")]
        [Compare("Password",
            ErrorMessage = "Пароль и подтверждение пароля не совпадают.")]
        public string ConfirmPassword { get; set; }
    }
}

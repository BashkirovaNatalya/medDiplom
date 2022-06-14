using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace med.ViewModels
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Roles = new List<UserRolesViewModel>();
        }

        public string Id { get; set; }

        [Required]
        [DisplayName("Имя пользователя")]

        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [DisplayName("E-mail")]

        public string Email { get; set; }

        [DisplayName("ФИО")]
        public string Fio { get; set; }
        [DisplayName("Номер телефона")]
        public string PhoneNumber { get; set; }
        [DisplayName("Адрес")]
        public string? Address { get; set; }
        [DisplayName("Должность")]

        public int EmployeePositionId { get; set; }

        [DisplayName("Роли пользователя")]

        public IList<UserRolesViewModel> Roles { get; set; }
    }
}

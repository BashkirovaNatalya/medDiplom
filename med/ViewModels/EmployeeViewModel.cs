using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace med.ViewModels
{
    public class EmployeeViewModel
    {
        public string Id { get; set; }

        [Required]
        [DisplayName("Пользователь")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [DisplayName("E-mail")]

        public string Email { get; set; }
        [DisplayName("ФИО")]

        public string Fio { get; set; }
        [DisplayName("Номер телефона")]

        public string PhoneNumber { get; set; }
        [DisplayName("Должность")]


        public string EmployeePositionName { get; set; }
    }
}

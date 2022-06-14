using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace med.ViewModels
{
    public class EditClientViewModel
    {
        public EditClientViewModel()
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
        
        [DisplayName("Должность")]

        public int ClientPositionId { get; set; }
        [DisplayName("Организация")]

        public int OrganizationId { get; set; }

        [DisplayName("Роли пользователя")]

        public IList<UserRolesViewModel> Roles { get; set; }
    }
}
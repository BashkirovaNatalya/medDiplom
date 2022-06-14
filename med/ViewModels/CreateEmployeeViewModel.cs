using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using med.Models;

namespace med.ViewModels
{
    public class CreateEmployeeViewModel
    {

        public int IdEmployee { get; set; }

        public string Fio { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        [DisplayName("Адрес")]
        public string? Address { get; set; }
        public int EmployeePositionId { get; set; }
        //public string ApplicationUserId { get; set; }
        
    }
}

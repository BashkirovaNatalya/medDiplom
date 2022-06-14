using System.ComponentModel.DataAnnotations;

namespace med.ViewModels
{
    public class ClientViewModel
    {
        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Fio { get; set; }
        public string PhoneNumber { get; set; }
        public string OrganizationName { get; set; }
        public string ClientPositionName { get; set; }

    }
}

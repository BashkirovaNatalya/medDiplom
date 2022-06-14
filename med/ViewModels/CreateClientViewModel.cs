using System.ComponentModel.DataAnnotations;
using med.Models;

namespace med.ViewModels
{
    public class CreateClientViewModel
    {
        public int IdClient { get; set; }
        public string Fio { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public int ClientPositionId { get; set; }
        //
    }
}

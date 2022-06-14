using System.ComponentModel.DataAnnotations;

namespace med.Models
{
    public class Client :IProfile
    {
        [Key] 
        public int IdClient { get; set; }

        public string Fio { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        public int  OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }

        public int ClientPositionId { get; set; }
        public virtual ClientPosition ClientPosition { get; set; }
        //
        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}

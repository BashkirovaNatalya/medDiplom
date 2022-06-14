using System.ComponentModel.DataAnnotations;

namespace med.Models
{
    public class Employee : IProfile
    {
        [Key]
        public int IdEmployee { get; set; }

        public string Fio { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public int EmployeePositionId { get; set; }
        public string? Address { get; set; }
        public virtual EmployeePosition EmployeePosition { get; set; } 
        public string ApplicationUserId { get; set; }
        //
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}

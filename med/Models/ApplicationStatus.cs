using System.ComponentModel.DataAnnotations;

namespace med.Models
{
    public class ApplicationStatus
    {
        [Key]
        public int IdApplicationStatus { get; set; }

        public string Name { get; set; }

    }
}

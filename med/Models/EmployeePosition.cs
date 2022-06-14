using System.ComponentModel.DataAnnotations;

namespace med.Models
{
    public class EmployeePosition
    {
        [Key]
        public int IdEmployeePosition { get; set; }
        public string Name { get; set; }
    }
}

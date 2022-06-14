using System.ComponentModel.DataAnnotations;

namespace med.Models
{
    public class License
    {
        [Key]
        public int IdLicense { get; set; }
        public string Number { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int EquipmentTypeId { get; set; }
        public EquipmentType EquipmentType { get; set; }
    }
}

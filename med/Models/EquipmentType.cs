using System.ComponentModel.DataAnnotations;

namespace med.Models
{
    public class EquipmentType
    {
        [Key]
        public int IdEquipmentType { get; set; }
        public string Name { get; set; }
    }
}

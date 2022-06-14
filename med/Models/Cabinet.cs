using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace med.Models
{
    public class Cabinet
    {
        [Key]
        public int IdCabinet { get; set; }
        [DisplayName("Номер")]
        public string Number { get; set; }
        [DisplayName("Тип")]
        public string Type { get; set; }
        [DisplayName("Филиал")]
        public int FilialId     { get; set; }
        public virtual Filial Filial    { get; set; }
    }
}

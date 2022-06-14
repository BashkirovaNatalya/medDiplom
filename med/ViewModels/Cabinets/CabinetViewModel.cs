using System.ComponentModel;

namespace med.ViewModels.Cabinets
{
    public class CabinetViewModel
    {
        public int IdCabinet { get; set; }
        [DisplayName("Номер")]
        public string Number { get; set; }
        [DisplayName("Тип")]
        public string Type { get; set; }
        [DisplayName("Филиал")]
        public string FilialName { get; set; }
    }
}

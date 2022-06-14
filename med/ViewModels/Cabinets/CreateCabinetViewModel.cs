using System.ComponentModel;

namespace med.ViewModels.Cabinets
{
    public class CreateCabinetViewModel
    {
        [DisplayName("Номер")]
        public string Number { get; set; }
        [DisplayName("Тип")]
        public string Type { get; set; }
        [DisplayName("Филиал")]
        public int FilialId { get; set; }
    }
}

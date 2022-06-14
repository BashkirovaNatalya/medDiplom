using System.ComponentModel;
using med.Models;

namespace med.ViewModels.Equipment
{
    public class EquipmentViewModel
    {
        public EquipmentViewModel()
        {
            ApplicationList = new List<Application>();
        }

        [DisplayName("")]
        public int IdEquipment { get; set; }
        [DisplayName("Название")]

        public string Name { get; set; }
        [DisplayName("Производитель")]

        public string Manufacturer { get; set; }
        [DisplayName("Серийный номер")]

        public string SerialNumber { get; set; }
        [DisplayName("Год выпуска")]

        public int ManufacturingYear { get; set; }
        [DisplayName("Номер сертификата")]

        public string CertificateNubber { get; set; }
        [DisplayName("Год ввода в эксплуатацию")]

        public int CommissioningYear { get; set; }
        [DisplayName("Год списания")]

        public int? DecommissioningYear { get; set; }
        [DisplayName("Период ТО")]

        public int PeriodTO { get; set; }
        [DisplayName("Тип")]

        public EquipmentType EquipmentType { get; set; }
        [DisplayName("Кабинет")]

        public Cabinet Cabinet { get; set; }
        [DisplayName("Ответственный")]


        public Client? Client { get; set; }

        public List<Application> ApplicationList { get; set; }
    }
}

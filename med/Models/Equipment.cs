using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace med.Models
{
    public class Equipment
    {
        [Key]
        public int IdEquipment { get; set; }
        [DisplayName("Название")]
        public string Name { get; set; }
        [DisplayName("Производитель")]
        public string Manufacturer  { get; set; }
        [DisplayName("Серийный номер")]
        public string SerialNumber  { get; set; }
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
        public int EquipmentTypeId { get; set; }
        [DisplayName("Тип")]

        public virtual EquipmentType EquipmentType { get; set; }
        [DisplayName("Кабинет")]
        public int CabinetId { get; set; }
        [DisplayName("Кабинет")]

        public virtual Cabinet Cabinet { get; set; }
        
        [DisplayName("Ответственный")]
        public int? ClientId { get; set; }
        [DisplayName("Ответственный")]

        public virtual Client Client { get; set; }


    }
}

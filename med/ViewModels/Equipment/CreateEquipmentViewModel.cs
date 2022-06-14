
namespace med.ViewModels.Equipment
{
    public class CreateEquipmentViewModel
    {
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string SerialNumber { get; set; }
        public int ManufacturingYear { get; set; }
        public string CertificateNubber { get; set; }
        public int CommissioningYear { get; set; }
        public int DecommissioningYear { get; set; }
        public int PeriodTO { get; set; }
        public int EquipmentTypeId { get; set; }
        public int CabinetId { get; set; }
        public int ClientId { get; set; }
    }
}

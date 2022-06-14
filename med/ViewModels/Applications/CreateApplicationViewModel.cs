using System.ComponentModel;

namespace med.ViewModels.Applications
{
    public class CreateApplicationViewModel
    {
        [DisplayName("Описание")]
        public string? Description { get; set; }
        [DisplayName("Начало план")]
        public DateTime? StartDatePlan { get; set; }
        //public DateTime? StartDateFact { get; set; }
        [DisplayName("Конец план")]
        public DateTime? FinishDatePlan { get; set; }
        //public DateTime? FinishDateFact { get; set; }
        [DisplayName("Оборудование")]
        public int EquipmentId { get; set; }
        //
        //public int? EmployeeId { get; set; }
        
        ////
        //public int ApplicationStatusId { get; set; }
    }
}

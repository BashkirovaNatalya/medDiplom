using System.ComponentModel;
using med.Models;

namespace med.ViewModels.Applications
{
    public class ApplicationViewModel
    {
        public int IdApplication { get; set; }
        [DisplayName("Описание")]
        public string? Description { get; set; }
        [DisplayName("Начало план")]

        public DateTime? StartDatePlan { get; set; }
        [DisplayName("Начало факт")]

        public DateTime? StartDateFact { get; set; }
        [DisplayName("Конец план")]

        public DateTime? FinishDatePlan { get; set; }
        [DisplayName("Конец факт")]

        public DateTime? FinishDateFact { get; set; }

        public int EquipmentId { get; set; }
        [DisplayName("Оборудование")]

        public virtual Models.Equipment Equipment { get; set; }

        public int? EmployeeId { get; set; }
        [DisplayName("Инженер")]

        public virtual Employee Employee { get; set; }
        //

        public int ApplicationStatusId { get; set; }
        [DisplayName("Статус")]

        public virtual ApplicationStatus ApplicationStatus { get; set; }

        public List<Models.Job> Jobs { get; set; }

    }
}

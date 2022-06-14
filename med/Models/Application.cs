using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace med.Models
{
    public class Application
    {
        [Key]
        public int IdApplication { get; set; }
        [DisplayName("Описание")]
        public string? Description { get; set; }
        [DisplayName("Начало")]

        public DateTime? StartDatePlan { get; set; }
        public DateTime? StartDateFact { get; set; }
        [DisplayName("Конец")]

        public DateTime? FinishDatePlan { get; set; }
        public DateTime? FinishDateFact { get; set; }
        [DisplayName("Оборудование")]

        public int EquipmentId { get; set; }
        [DisplayName("Оборудование")]

        public virtual Equipment Equipment { get; set; }
        [DisplayName("Инженер")]
        public int? EmployeeId { get; set; }
        [DisplayName("Инженер")]

        public virtual Employee Employee { get; set; }
        //
        [DisplayName("Статус")]

        public int ApplicationStatusId { get; set; }
        [DisplayName("Статус")]

        public virtual ApplicationStatus ApplicationStatus { get; set; }
    }
}

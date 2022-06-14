using med.Models;

namespace med.ViewModels.Applications
{
    public class EditApplicationViewModel
    {
        public int IdApplication { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDatePlan { get; set; }
        public DateTime? FinishDatePlan { get; set; }
        public int EquipmentId { get; set; }
        public int ApplicationStatusId { get; set; }
    }
}

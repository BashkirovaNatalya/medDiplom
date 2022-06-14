using System.ComponentModel;

namespace med.ViewModels.Job
{
    public class CreateJobViewModel
    {


        [DisplayName("Описание")]
        public string Description { get; set; }
        [DisplayName("Стоимость")]

        public double Cost { get; set; }
        [DisplayName("Начало")]

        public DateTime? StartDatePlan { get; set; }
        //public DateTime? StartDateFact { get; set; }
        [DisplayName("Конец")]


        public DateTime? FinishDatePlan { get; set; }
        //public DateTime? FinishDateFact { get; set; }
        [DisplayName("Заявка выполнена")]

        public bool IsCompleted { get; set; }
        //
        [DisplayName("Тип работы")]

        public int JobTypeId { get; set; }
        [DisplayName("Фотоотчет")]

        public IFormFileCollection? Photos { get; set; }
        //
    }
}

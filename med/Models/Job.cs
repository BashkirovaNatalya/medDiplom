using System.ComponentModel.DataAnnotations;
using med.Data;

namespace med.Models
{
    public class Job
    {
        public Job()
        {
            Images = new List<JobImage>();
        }
        [Key]
        public int IdJob { get; set; }
        public string Description { get; set; }
        public double Cost  { get; set; }
        public DateTime? StartDatePlan { get; set; }
        public DateTime? StartDateFact { get; set; }
        public DateTime? FinishDatePlan { get; set; }
        public DateTime? FinishDateFact { get; set; }

        //

        public int JobTypeId { get; set; }
        public virtual JobType JobType { get; set; }
        //
        public int ApplicationId { get; set; }
        public virtual Application Application { get; set; }

        public List<JobImage>? Images { get; set; }
        // 
        //public int StageId { get; set; }
        //public virtual Stage Stage { get; set; }



    }
}

using System.ComponentModel.DataAnnotations;

namespace med.Models
{
    public class JobImage
    {
        [Key] 
        public int IdJobImage { get; set; }
        public string FilePath { get; set; }
        //
        public int JobId { get; set; }
        public virtual Job Job { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace med.Data
{
    public class JobType
    {
        [Key]
        public int IdJobType { get; set; }
        public string Name { get; set; }
    }
}

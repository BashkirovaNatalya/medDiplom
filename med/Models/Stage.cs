using System.ComponentModel.DataAnnotations;

namespace med.Models
{
    public class Stage
    {
        [Key]
        public int IdStage { get; set; }
        public string Name { get; set; }
    }
}

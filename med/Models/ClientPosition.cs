using System.ComponentModel.DataAnnotations;

namespace med.Models
{
    public class ClientPosition
    {
        [Key]
        public int IdClientPosition { get; set; }

        public string Name { get; set; }    
    }
}

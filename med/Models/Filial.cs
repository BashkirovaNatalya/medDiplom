using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace med.Models
{
    public class Filial
    {
        [Key] 
        public int IdFilial { get; set; }
        [DisplayName("Адрес")]
        public string Address { get; set; }
        [DisplayName("Название")]
        public string Name { get; set; }
        [DisplayName("Организация")]
        public int OrganizationId { get; set; }
        [DisplayName("Организация")]

        public virtual Organization Organization { get; set; }
    }
}

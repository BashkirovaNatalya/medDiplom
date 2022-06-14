using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace med.Models
{
    public class Organization
    {
        [Key]
        public int IdOrganization { get; set; }
        [DisplayName("Название")]

        public string Name { get; set; }
        [DisplayName("Адрес")]

        public string Address { get; set; }
        [DisplayName("Номер телефона")]

        public string PhoneNumber { get; set; }
        [DisplayName("Веб-сайт")]

        public string WebSite { get; set; }
        [DisplayName("ОКПО")]
        public string OKPO { get; set; }
        [DisplayName("ОКВЭД")] 
        public string OKVED { get; set; }
        [DisplayName("ИНН")] 
        public string INN { get; set; }
        [DisplayName("Корр. счет")]
        public string CorrAcc { get; set; }
        [DisplayName("БИК")]
        public string BIK { get; set; }
        [DisplayName("КПП")]
        public string KPP { get; set; }
        [DisplayName("ОГРН")]
        public string OGRN { get; set; }

    }
}

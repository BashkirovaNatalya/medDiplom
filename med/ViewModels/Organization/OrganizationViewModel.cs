using System.ComponentModel;

namespace med.ViewModels.Organization
{
    public class OrganizationViewModel
    {
        public int IdOrganization { get; set; }
        [DisplayName("Название")]
        public string Name { get; set; }
        [DisplayName("Адрес")]
        public string Address { get; set; }
        [DisplayName("Номер телефона")]
        public string PhoneNumber { get; set; }
        [DisplayName("Веб-сайт")]
        public string WebSite { get; set; }

    }
}

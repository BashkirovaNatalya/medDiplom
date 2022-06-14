using System.ComponentModel;

namespace med.ViewModels.Filials
{
    public class CreateFilialViewModel
    {
        [DisplayName("Адрес")]
        public string Address { get; set; }
        [DisplayName("Название")]
        public string Name { get; set; }
        [DisplayName("Организация")]
        public int OrganizationId { get; set; }
    }
}

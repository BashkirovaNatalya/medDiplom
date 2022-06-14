using System.ComponentModel;

namespace med.ViewModels.Filials
{
    public class FilialViewModel
    {
        public int IdFilial { get; set; }
        [DisplayName("Адрес")]
        public string Address { get; set; }
        [DisplayName("Название")]
        public string Name { get; set; }
        [DisplayName("Организация")]
        public string OrganizationName { get; set; }
    }
}

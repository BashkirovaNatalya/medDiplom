using System.ComponentModel.DataAnnotations;

namespace med.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}

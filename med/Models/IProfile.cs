namespace med.Models
{
    public class IProfile
    {
        public string Fio { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}

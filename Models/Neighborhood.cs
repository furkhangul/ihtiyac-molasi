using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IhtiyacMolasi.Models
{
    [Table("Neighborhoods")]
    public class Neighborhood
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        [Display(Name = "Mahalle Adı")]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        [Display(Name = "Şehir")]
        public string City { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        [Display(Name = "İlçe")]
        public string District { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public virtual ICollection<HelpRequest> HelpRequests { get; set; } = new List<HelpRequest>();
        public virtual ICollection<User> Users { get; set; } = new List<User>();

        [NotMapped]
        public string FullName => $"{Name}, {District} / {City}";
    }
}


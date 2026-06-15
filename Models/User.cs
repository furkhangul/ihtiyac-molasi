using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IhtiyacMolasi.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(80)]
        [Display(Name = "Rumuz")]
        public string Alias { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string SessionToken { get; set; } = string.Empty;

        public int? NeighborhoodId { get; set; }
        [ForeignKey("NeighborhoodId")]
        public virtual Neighborhood? Neighborhood { get; set; }

        public bool IsVolunteer { get; set; } = false;

        [Display(Name = "Gönüllü Puanı")]
        public int VolunteerScore { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastSeen { get; set; } = DateTime.UtcNow;

        public virtual ICollection<HelpRequest> CreatedRequests { get; set; } = new List<HelpRequest>();
        public virtual ICollection<VolunteerAssignment> Assignments { get; set; } = new List<VolunteerAssignment>();
    }
}
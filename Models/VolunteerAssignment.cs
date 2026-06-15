using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IhtiyacMolasi.Models
{
    public enum AssignmentStatus
    {
        Aktif = 0,
        Tamamlandı = 1,
        Vazgeçildi = 2
    }

    [Table("VolunteerAssignments")]
    public class VolunteerAssignment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int HelpRequestId { get; set; }
        [ForeignKey("HelpRequestId")]
        public virtual HelpRequest? HelpRequest { get; set; }

        public int? VolunteerUserId { get; set; }
        [ForeignKey("VolunteerUserId")]
        public virtual User? Volunteer { get; set; }

        [Required, MaxLength(80)]
        [Display(Name = "Gönüllü Adı")]
        public string VolunteerAlias { get; set; } = string.Empty;

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
        public AssignmentStatus Status { get; set; } = AssignmentStatus.Aktif;

        [MaxLength(500)]
        [Display(Name = "Not")]
        public string? Note { get; set; }
    }
}

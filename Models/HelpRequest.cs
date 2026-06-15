using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IhtiyacMolasi.Models
{
    public enum RequestStatus
    {
        Açık = 0,
        Üstlenildi = 1,
        Tamamlandı = 2,
        İptal = 3
    }

    public enum UrgencyLevel
    {
        Normal = 1,
        Acil = 2,
        ÇokAcil = 3
    }

    [Table("HelpRequests")]
    public class HelpRequest
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        [Display(Name = "Başlık")]
        public string Title { get; set; } = string.Empty;

        [Required, MaxLength(1000)]
        [Display(Name = "Açıklama")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        [Required]
        public int NeighborhoodId { get; set; }
        [ForeignKey("NeighborhoodId")]
        public virtual Neighborhood? Neighborhood { get; set; }

        public int? CreatedByUserId { get; set; }
        [ForeignKey("CreatedByUserId")]
        public virtual User? CreatedBy { get; set; }

        [MaxLength(80)]
        [Display(Name = "Ad / Rumuz")]
        public string RequesterAlias { get; set; } = "Anonim";

        public RequestStatus Status { get; set; } = RequestStatus.Açık;
        public UrgencyLevel UrgencyLevel { get; set; } = Models.UrgencyLevel.Normal;

        [Required]
        [Display(Name = "Geçerlilik Süresi")]
        public DateTime ExpiresAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public int ViewCount { get; set; } = 0;
        public bool IsAnonymous { get; set; } = true;

        public virtual ICollection<VolunteerAssignment> Assignments { get; set; } = new List<VolunteerAssignment>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

        // ── Hesaplama özellikleri ──
        [NotMapped]
        public bool IsExpired => ExpiresAt < DateTime.UtcNow;

        [NotMapped]
        public TimeSpan TimeLeft => ExpiresAt - DateTime.UtcNow;

        [NotMapped]
        public string TimeLeftDisplay
        {
            get
            {
                if (IsExpired) return "Süresi Doldu";
                var t = TimeLeft;
                if (t.TotalHours >= 1) return $"{(int)t.TotalHours} saat kaldı";
                return $"{t.Minutes} dakika kaldı";
            }
        }

        [NotMapped]
        public string UrgencyBadgeClass => UrgencyLevel switch
        {
            Models.UrgencyLevel.Acil => "badge-urgency-high",
            Models.UrgencyLevel.ÇokAcil => "badge-urgency-critical",
            _ => "badge-urgency-normal"
        };

        [NotMapped]
        public string StatusBadgeClass => Status switch
        {
            RequestStatus.Açık => "status-open",
            RequestStatus.Üstlenildi => "status-assigned",
            RequestStatus.Tamamlandı => "status-done",
            _ => "status-cancelled"
        };
    }
}


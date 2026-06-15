using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IhtiyacMolasi.Models
{
    [Table("Reports")]
    public class Report
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int HelpRequestId { get; set; }

        [ForeignKey(nameof(HelpRequestId))]
        public virtual HelpRequest? HelpRequest { get; set; }

        [MaxLength(80)]
        public string? ReporterAlias { get; set; }

        [Required]
        [MaxLength(300)]
        [Display(Name = "Şikayet Nedeni")]
        public string Reason { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsReviewed { get; set; } = false;
    }
}
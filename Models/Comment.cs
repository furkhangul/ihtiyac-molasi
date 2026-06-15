using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IhtiyacMolasi.Models
{
    [Table("Comments")]
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int HelpRequestId { get; set; }
        [ForeignKey("HelpRequestId")]
        public virtual HelpRequest? HelpRequest { get; set; }

        public int? AuthorUserId { get; set; }
        [ForeignKey("AuthorUserId")]
        public virtual User? Author { get; set; }

        [MaxLength(80)]
        public string AuthorAlias { get; set; } = "Anonim";

        [Required, MaxLength(500)]
        [Display(Name = "Yorum")]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
    }
}

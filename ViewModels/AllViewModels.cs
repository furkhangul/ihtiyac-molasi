// ============================================================
// ViewModels/HomeViewModel.cs
// ============================================================
using IhtiyacMolasi.Models;

namespace IhtiyacMolasi.ViewModels
{
    public class HomeViewModel
    {
        public List<HelpRequest> ActiveRequests   { get; set; } = new();
        public List<Category>    Categories        { get; set; } = new();
        public List<Neighborhood> Neighborhoods    { get; set; } = new();
        public int               TotalRequests     { get; set; }
        public int               TotalVolunteers   { get; set; }
        public int               TotalCompleted    { get; set; }

        // Filtreler
        public int?    FilterCategoryId     { get; set; }
        public int?    FilterNeighborhoodId { get; set; }
        public string? FilterUrgency        { get; set; }
        public string? SearchQuery          { get; set; }
    }
}

// ============================================================
// ViewModels/CreateRequestViewModel.cs
// ============================================================
namespace IhtiyacMolasi.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class CreateRequestViewModel
    {
        [Required(ErrorMessage = "Başlık zorunludur.")]
        [MaxLength(200, ErrorMessage = "Başlık 200 karakterden uzun olamaz.")]
        [Display(Name = "Başlık")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        [MaxLength(1000, ErrorMessage = "Açıklama 1000 karakterden uzun olamaz.")]
        [Display(Name = "Açıklama")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kategori seçiniz.")]
        [Display(Name = "Kategori")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Mahalle seçiniz.")]
        [Display(Name = "Mahalle")]
        public int NeighborhoodId { get; set; }

        [MaxLength(80)]
        [Display(Name = "Adınız / Rumuzunuz (opsiyonel)")]
        public string? RequesterAlias { get; set; }

        [Required(ErrorMessage = "Aciliyet seviyesi seçiniz.")]
        [Display(Name = "Aciliyet")]
        public UrgencyLevel UrgencyLevel { get; set; } = UrgencyLevel.Normal;

        [Required(ErrorMessage = "Geçerlilik süresi seçiniz.")]
        [Display(Name = "Ne kadar süre geçerli?")]
        public int ExpiresInHours { get; set; } = 2;

        public bool IsAnonymous { get; set; } = true;

        // Dropdown doldurmak için
        public List<Category>     Categories    { get; set; } = new();
        public List<Neighborhood> Neighborhoods { get; set; } = new();
    }
}

// ============================================================
// ViewModels/RequestDetailViewModel.cs
// ============================================================
namespace IhtiyacMolasi.ViewModels
{
    public class RequestDetailViewModel
    {
        public HelpRequest          Request        { get; set; } = null!;
        public List<Comment>        Comments       { get; set; } = new();
        public List<VolunteerAssignment> Assignments { get; set; } = new();
        public string               NewComment     { get; set; } = string.Empty;
        public string               VolunteerAlias { get; set; } = string.Empty;
    }
}

// ============================================================
// ViewModels/VolunteerViewModel.cs
// ============================================================
namespace IhtiyacMolasi.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class VolunteerViewModel
    {
        [Required(ErrorMessage = "Talep ID gereklidir.")]
        public int HelpRequestId { get; set; }

        [Required(ErrorMessage = "Adınızı veya rumuzunuzu giriniz.")]
        [MaxLength(80, ErrorMessage = "Ad 80 karakterden uzun olamaz.")]
        [Display(Name = "Adınız / Rumuzunuz")]
        public string VolunteerAlias { get; set; } = string.Empty;

        [MaxLength(500)]
        [Display(Name = "Not (opsiyonel)")]
        public string? Note { get; set; }

        public HelpRequest? Request { get; set; }
    }
}

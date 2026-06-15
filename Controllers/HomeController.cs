using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IhtiyacMolasi.Data;
using IhtiyacMolasi.Models;
using IhtiyacMolasi.ViewModels;

namespace IhtiyacMolasi.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ILogger<HomeController> _logger;

        public HomeController(AppDbContext db, ILogger<HomeController> logger)
        {
            _db = db;
            _logger = logger;
        }

        // GET /
        public async Task<IActionResult> Index(
            int? categoryId = null,
            int? neighborhoodId = null,
            string? urgency = null,
            string? q = null)
        {
            var query = _db.HelpRequests
                .Include(r => r.Category)
                .Include(r => r.Neighborhood)
                .Include(r => r.Comments)
                .Include(r => r.Assignments)
                .Where(r => r.Status == RequestStatus.Açık || r.Status == RequestStatus.Üstlenildi)
                .Where(r => r.ExpiresAt > DateTime.UtcNow)
                .AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(r => r.CategoryId == categoryId.Value);

            if (neighborhoodId.HasValue)
                query = query.Where(r => r.NeighborhoodId == neighborhoodId.Value);

            if (!string.IsNullOrWhiteSpace(urgency) && Enum.TryParse<UrgencyLevel>(urgency, out var ul))
                query = query.Where(r => r.UrgencyLevel == ul);

            if (!string.IsNullOrWhiteSpace(q))
                query = query.Where(r => r.Title.Contains(q) || r.Description.Contains(q));

            var requests = await query
                .OrderByDescending(r => r.UrgencyLevel)
                .ThenByDescending(r => r.CreatedAt)
                .Take(50)
                .ToListAsync();

            var vm = new HomeViewModel
            {
                ActiveRequests = requests,
                Categories = await _db.Categories.Where(c => c.IsActive).ToListAsync(),
                Neighborhoods = await _db.Neighborhoods.Where(n => n.IsActive).ToListAsync(),
                TotalRequests = await _db.HelpRequests.CountAsync(),
                TotalVolunteers = await _db.VolunteerAssignments.Select(v => v.VolunteerAlias).Distinct().CountAsync(),
                TotalCompleted = await _db.HelpRequests.CountAsync(r => r.Status == RequestStatus.Tamamlandı),
                FilterCategoryId = categoryId,
                FilterNeighborhoodId = neighborhoodId,
                FilterUrgency = urgency,
                SearchQuery = q
            };

            return View(vm);
        }

        // GET /Home/About
        public IActionResult About() => View();

        // GET /Home/Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View();
    }
}
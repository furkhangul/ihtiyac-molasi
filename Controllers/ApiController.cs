using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IhtiyacMolasi.Data;
using IhtiyacMolasi.Models;
using IhtiyacMolasi.ViewModels;
namespace IhtiyacMolasi.Controllers
{
    [Route("api")]
    [ApiController]
    public class DataApiController : ControllerBase
    {
        private readonly AppDbContext _db;
        public DataApiController(AppDbContext db) => _db = db;

        // GET /api/requests/live  - canlı talep listesi (AJAX polling)
        [HttpGet("requests/live")]
        public async Task<IActionResult> LiveRequests(
            int? categoryId = null,
            int? neighborhoodId = null)
        {
            var q = _db.HelpRequests
                .Include(r => r.Category)
                .Include(r => r.Neighborhood)
                .Where(r => (r.Status == RequestStatus.Açık || r.Status == RequestStatus.Üstlenildi)
                         && r.ExpiresAt > DateTime.UtcNow);

            if (categoryId.HasValue)     q = q.Where(r => r.CategoryId == categoryId);
            if (neighborhoodId.HasValue) q = q.Where(r => r.NeighborhoodId == neighborhoodId);

            var list = await q
                .OrderByDescending(r => r.UrgencyLevel)
                .ThenByDescending(r => r.CreatedAt)
                .Take(30)
                .Select(r => new
                {
                    r.Id, r.Title, r.RequesterAlias, r.IsAnonymous,
                    r.ViewCount, r.CreatedAt,
                    Status      = r.Status.ToString(),
                    Urgency     = r.UrgencyLevel.ToString(),
                    Category    = r.Category!.Name,
                    Icon        = r.Category.Icon,
                    Color       = r.Category.ColorHex,
                    Neighborhood = r.Neighborhood!.Name,
                    ExpiresAt   = r.ExpiresAt,
                    CommentCount = r.Comments.Count(c => !c.IsDeleted)
                })
                .ToListAsync();

            return Ok(list);
        }

        // GET /api/stats
        [HttpGet("stats")]
        public async Task<IActionResult> Stats()
        {
            return Ok(new
            {
                Total     = await _db.HelpRequests.CountAsync(),
                Open      = await _db.HelpRequests.CountAsync(r => r.Status == RequestStatus.Açık),
                Completed = await _db.HelpRequests.CountAsync(r => r.Status == RequestStatus.Tamamlandı),
                Volunteers = await _db.VolunteerAssignments.Select(v => v.VolunteerAlias).Distinct().CountAsync()
            });
        }
    }
}

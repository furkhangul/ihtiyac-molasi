using IhtiyacMolasi.Data;
using IhtiyacMolasi.Models;
using IhtiyacMolasi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace IhtiyacMolasi.Controllers
{
    public class RequestController : Controller
    {
        private readonly AppDbContext _db;

        public RequestController(AppDbContext db) => _db = db;

        // GET /Request/Create
        public async Task<IActionResult> Create()
        {
            var vm = new CreateRequestViewModel
            {
                Categories = await _db.Categories.Where(c => c.IsActive).ToListAsync(),
                Neighborhoods = await _db.Neighborhoods.Where(n => n.IsActive).ToListAsync()
            };
            return View(vm);
        }

        // POST /Request/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRequestViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Categories = await _db.Categories.Where(c => c.IsActive).ToListAsync();
                vm.Neighborhoods = await _db.Neighborhoods.Where(n => n.IsActive).ToListAsync();
                return View(vm);
            }

            var request = new HelpRequest
            {
                Title = vm.Title.Trim(),
                Description = vm.Description.Trim(),
                CategoryId = vm.CategoryId,
                NeighborhoodId = vm.NeighborhoodId,
                RequesterAlias = string.IsNullOrWhiteSpace(vm.RequesterAlias) ? "Anonim" : vm.RequesterAlias.Trim(),
                UrgencyLevel = vm.UrgencyLevel,
                ExpiresAt = DateTime.UtcNow.AddHours(vm.ExpiresInHours),
                IsAnonymous = vm.IsAnonymous || string.IsNullOrWhiteSpace(vm.RequesterAlias)
            };

            _db.HelpRequests.Add(request);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Yardım talebiniz oluşturuldu! Gönüllüler en kısa sürede üstlenecek.";
            return RedirectToAction("Detail", new { id = request.Id });
        }

        // GET /Request/Detail/5
        public async Task<IActionResult> Detail(int id)
        {
            var request = await _db.HelpRequests
                .Include(r => r.Category)
                .Include(r => r.Neighborhood)
                .Include(r => r.Comments.Where(c => !c.IsDeleted).OrderBy(c => c.CreatedAt))
                .Include(r => r.Assignments.OrderByDescending(a => a.AssignedAt))
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null) return NotFound();

            // Görüntülenme sayısını artır
            request.ViewCount++;
            await _db.SaveChangesAsync();

            var vm = new RequestDetailViewModel
            {
                Request = request,
                Comments = request.Comments.ToList(),
                Assignments = request.Assignments.ToList()
            };

            return View(vm);
        }

        // POST /Request/AddComment
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int requestId, string content, string? alias)
        {
            if (string.IsNullOrWhiteSpace(content) || content.Length > 500)
            {
                TempData["Error"] = "Yorum 1-500 karakter arasında olmalıdır.";
                return RedirectToAction("Detail", new { id = requestId });
            }

            _db.Comments.Add(new Comment
            {
                HelpRequestId = requestId,
                AuthorAlias = string.IsNullOrWhiteSpace(alias) ? "Anonim" : alias.Trim(),
                Content = content.Trim()
            });

            await _db.SaveChangesAsync();
            TempData["Success"] = "Yorumunuz eklendi.";
            return RedirectToAction("Detail", new { id = requestId });
        }

        // POST /Request/Report
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Report(int requestId, string reason, string? alias)
        {
            if (string.IsNullOrWhiteSpace(reason))
            {
                TempData["Error"] = "Şikayet nedeni belirtiniz.";
                return RedirectToAction("Detail", new { id = requestId });
            }

            _db.Reports.Add(new Report
            {
                HelpRequestId = requestId,
                ReporterAlias = alias,
                Reason = reason.Trim()
            });

            await _db.SaveChangesAsync();
            TempData["Info"] = "Şikayetiniz alındı, incelenecek.";
            return RedirectToAction("Detail", new { id = requestId });
        }

        // POST /Request/Cancel
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var req = await _db.HelpRequests.FindAsync(id);
            if (req == null) return NotFound();

            req.Status = RequestStatus.İptal;
            req.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            TempData["Info"] = "Talep iptal edildi.";
            return RedirectToAction("Index", "Home");
        }
    }
}

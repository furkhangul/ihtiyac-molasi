using IhtiyacMolasi.Data;
using IhtiyacMolasi.Models;
using IhtiyacMolasi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace IhtiyacMolasi.Controllers
{
    public class VolunteerController : Controller
    {
        private readonly AppDbContext _db;

        public VolunteerController(AppDbContext db) => _db = db;

        // GET /Volunteer/TakeRequest/5
        public async Task<IActionResult> TakeRequest(int id)
        {
            var request = await _db.HelpRequests
                .Include(r => r.Category)
                .Include(r => r.Neighborhood)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null) return NotFound();
            if (request.Status != RequestStatus.Açık)
            {
                TempData["Error"] = "Bu talep artık mevcut değil.";
                return RedirectToAction("Index", "Home");
            }

            return View(new VolunteerViewModel { HelpRequestId = id, Request = request });
        }

        // POST /Volunteer/TakeRequest
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> TakeRequest(VolunteerViewModel vm)
        {
            var request = await _db.HelpRequests
                .Include(r => r.Category)
                .Include(r => r.Neighborhood)
                .FirstOrDefaultAsync(r => r.Id == vm.HelpRequestId);

            if (request == null) return NotFound();

            if (!ModelState.IsValid)
            {
                vm.Request = request;
                return View(vm);
            }

            if (request.Status != RequestStatus.Açık)
            {
                TempData["Error"] = "Bu talep zaten üstlenilmiş.";
                return RedirectToAction("Detail", "Request", new { id = vm.HelpRequestId });
            }

            // Atama oluştur
            _db.VolunteerAssignments.Add(new VolunteerAssignment
            {
                HelpRequestId = vm.HelpRequestId,
                VolunteerAlias = vm.VolunteerAlias.Trim(),
                Note = vm.Note?.Trim(),
                AssignedAt = DateTime.UtcNow
            });

            // Talebi güncelle
            request.Status = RequestStatus.Üstlenildi;
            request.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            TempData["Success"] = $"Teşekkürler {vm.VolunteerAlias}! Talebi üstlendiniz. İhtiyaç sahibine ulaşabilirsiniz.";
            return RedirectToAction("Detail", "Request", new { id = vm.HelpRequestId });
        }

        // POST /Volunteer/Complete
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(int assignmentId)
        {
            var assignment = await _db.VolunteerAssignments
                .Include(a => a.HelpRequest)
                .FirstOrDefaultAsync(a => a.Id == assignmentId);

            if (assignment == null) return NotFound();

            assignment.Status = AssignmentStatus.Tamamlandı;
            assignment.CompletedAt = DateTime.UtcNow;

            if (assignment.HelpRequest != null)
            {
                assignment.HelpRequest.Status = RequestStatus.Tamamlandı;
                assignment.HelpRequest.UpdatedAt = DateTime.UtcNow;
            }

            await _db.SaveChangesAsync();
            TempData["Success"] = "Harika! Yardım tamamlandı olarak işaretlendi. 🎉";
            return RedirectToAction("Detail", "Request", new { id = assignment.HelpRequestId });
        }
    }
}
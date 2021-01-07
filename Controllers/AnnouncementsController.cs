using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnnouncementsApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace AnnouncementsApp.Controllers
{
    public class AnnouncementsController : Controller
    {
        private AnnouncementContext db;

        public AnnouncementsController(AnnouncementContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            var model = db.Announcement.Include(e => e.Category).OrderBy(somebody => somebody.Title);
            return View(model);
        }

        public async Task<IActionResult> DetailsAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            // Include sluzy do "załadowania" tabeli Category.
            var model = await db.Announcement.Include(e => e.Category).FirstOrDefaultAsync(acc => acc.AnnouncementId == id);
            if (model != null)
            {
                return View(model);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        // Tylko dla zajerestrowanych akcja.
        [Authorize]
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Category, "CategoryId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Tylko dla zajerestrowanych akcja.
        [Authorize]
        public async Task<IActionResult> Create(Announcement announcement)
        {
            // Zastosowanie walidacji, sprawdzneie czy wszystko jest ok.
            if (ModelState.IsValid)
            {
                announcement.AddedDate = DateTime.Now;

                // Email zalogowanego usera
                announcement.Mail = User.Identity.Name;
                db.Announcement.Add(announcement);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Przekierowujemy na akcje z listą
            }
            ViewBag.CategoryId = new SelectList(db.Category, "CategoryId", "Name"); // Musimy ponownie zwrocic liste kategorii, jesli formularz nie jest poprawny

            // Jeśli nie jest poprawne, to zwracamy te samą stronę z wpisanymi danymi.
            return View(announcement);
        }


    }
}

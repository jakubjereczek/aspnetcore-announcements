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

        public IActionResult Index(int? id = null)
        {
            ViewBag.Categories = new SelectList(db.Category, "CategoryId", "Name");

            // Ciasteczko z wpisanym miastem
            ViewBag.City = GetCity().City;
            // Model FilterCity potrzebny do wygenrowania formularza
            ViewBag.FilterCity = new FilterCity();

            // Brak kategorii - wyswietlenie wszystkiego
            if (id == null || id == 0)
            {
                var model = db.Announcement.Include(e => e.Category).OrderBy(somebody => somebody.Title);
                return View(model);
            }
            else
            {
                // Wyswietlenie dla kategorii
                var model = db.Announcement.Include(e => e.Category).Where(somebody => somebody.CategoryId == id);
                return View(model);
            }
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

        [HttpGet]
        [Authorize]
        // Wyswietlanie modelu na podstawie ID - dla autora ogloszenia.
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var model = await db.Announcement.Include(e => e.Category).FirstOrDefaultAsync(acc => acc.AnnouncementId == id);
            if (model != null)
            {
                // Jesli autorem wpisu NIE jest zalogowany
                if (model.Mail != User.Identity.Name)
                {
                    return NotFound();
                }

                return View(model);
            }
            return NotFound();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var model = await db.Announcement.FindAsync(Int32.Parse(id));
                // Jesli autorem wpisu jest zalogowany
                if (model.Mail == User.Identity.Name)
                {
                    db.Announcement.Remove(model);
                    await db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return NotFound();

            }
            catch (DbUpdateException err)
            {
                return RedirectToAction("Delete", new
                {
                    Id = id,
                    saveChangesError = true
                });
            }

        }



        [HttpGet]
        [Authorize]
        // Wyswietlanie modelu na podstawie ID - dla autora ogloszenia.
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            var model = await db.Announcement.Include(e => e.Category).FirstOrDefaultAsync(acc => acc.AnnouncementId == id);
            if (model != null)
            {
                // Jesli autorem wpisu NIE jest zalogowany
                if (model.Mail != User.Identity.Name)
                    return NotFound();

                ViewBag.CategoryId = new SelectList(db.Category, "CategoryId", "Name");
                return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Announcement announcement)
        {
            ViewBag.CategoryId = new SelectList(db.Category, "CategoryId", "Name");

            if (ModelState.IsValid && id != null)
            {
                try
                {
                    announcement.AnnouncementId = (int)id; // ID JAK POZOSTALE
                    db.Announcement.Update(announcement);
                    //db.Entry(announcement).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Details", new { id });
                }
                catch (DbUpdateException err)
                {
                    ModelState.AddModelError("", "Bląd z edycją danych");
                }
            }
            var model = await db.Announcement.FindAsync(id);
            return View(model);
        }



        // CIASTECZKA 
        // Zastosowanie mechanizmu ciasterczek - dla miasta, po którym będziemy potem filtrować.

        [HttpPost]
        public ActionResult RefreshCity(FilterCity filterCity)
        {
            if (filterCity.City == null) {
                filterCity.City = "";
            }
            var city = GetCity();
            city.City = filterCity.City;
            SetCity(city);

            return RedirectToAction("Index");
        }

        public FilterCity GetCity()
        {
            var filterCity = new FilterCity();

            if (Request.Cookies["city"] != null)
            {
                filterCity.City = Request.Cookies["city"].ToString();
            }
            else
            {
                filterCity.City = "";
            }
            return filterCity;
        }

        public void SetCity(FilterCity filterCity)
        {
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(1); // Ciasteczko istnieje dzień, a bez tego jest tymczasowe - na czas trwania apk.
            Response.Cookies.Append("city", filterCity.City, options);
        }
    }

}

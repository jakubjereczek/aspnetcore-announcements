using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnnouncementsApp.Models;

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


        /*        // GET: api/Announcements
                [HttpGet]
                public async Task<ActionResult<IEnumerable<Announcement>>> GetAnnouncement()
                {
                    return await _context.Announcement.ToListAsync();
                }

                // GET: api/Announcements/5
                [HttpGet("{id}")]
                public async Task<ActionResult<Announcement>> GetAnnouncement(int id)
                {
                    var announcement = await _context.Announcement.FindAsync(id);

                    if (announcement == null)
                    {
                        return NotFound();
                    }

                    return announcement;
                }

                // PUT: api/Announcements/5
                // To protect from overposting attacks, enable the specific properties you want to bind to, for
                // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
                [HttpPut("{id}")]
                public async Task<IActionResult> PutAnnouncement(int id, Announcement announcement)
                {
                    if (id != announcement.AnnouncementId)
                    {
                        return BadRequest();
                    }

                    _context.Entry(announcement).State = EntityState.Modified;

                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!AnnouncementExists(id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    return NoContent();
                }

                // POST: api/Announcements
                // To protect from overposting attacks, enable the specific properties you want to bind to, for
                // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
                [HttpPost]
                public async Task<ActionResult<Announcement>> PostAnnouncement(Announcement announcement)
                {
                    _context.Announcement.Add(announcement);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetAnnouncement", new { id = announcement.AnnouncementId }, announcement);
                }

                // DELETE: api/Announcements/5
                [HttpDelete("{id}")]
                public async Task<ActionResult<Announcement>> DeleteAnnouncement(int id)
                {
                    var announcement = await _context.Announcement.FindAsync(id);
                    if (announcement == null)
                    {
                        return NotFound();
                    }

                    _context.Announcement.Remove(announcement);
                    await _context.SaveChangesAsync();

                    return announcement;
                }

                private bool AnnouncementExists(int id)
                {
                    return _context.Announcement.Any(e => e.AnnouncementId == id);
                }*/
    }
}

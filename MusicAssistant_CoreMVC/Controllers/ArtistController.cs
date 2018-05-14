using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicAssistantMvcCore.Models;
using MusicAssistant_CoreMVC.Data;
using Microsoft.AspNetCore.Authorization;

namespace MusicAssistant_CoreMVC.Controllers
{
    public class ArtistController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArtistController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Artist
        public async Task<IActionResult> Index()
        {
            return View(await _context.Artists.ToListAsync());
        }

        // GET: Artist/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artistModel = await _context.Artists
                .SingleOrDefaultAsync(m => m.Id == id);
            if (artistModel == null)
            {
                return NotFound();
            }

            _context.Entry(artistModel).Collection(x => x.UserFollow).Load();

            return View(artistModel);
        }

        // GET: Artist/Create
        [Authorize(Roles = "Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Artist/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Pseudonym,Name,LastName,BirthDate,DeathDate,CareerStart,CareerEnd,BirthPlace,Gender,Biography,ArtistPhotoUrl")] ArtistModel artistModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(artistModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(artistModel);
        }

        // GET: Artist/Edit/5
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artistModel = await _context.Artists.SingleOrDefaultAsync(m => m.Id == id);
            if (artistModel == null)
            {
                return NotFound();
            }
            return View(artistModel);
        }

        // POST: Artist/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Pseudonym,Name,LastName,BirthDate,DeathDate,CareerStart,CareerEnd,BirthPlace,Gender,Biography,ArtistPhotoUrl")] ArtistModel artistModel)
        {
            if (id != artistModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(artistModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtistModelExists(artistModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(artistModel);
        }

        // GET: Artist/Delete/5
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artistModel = await _context.Artists
                .SingleOrDefaultAsync(m => m.Id == id);
            if (artistModel == null)
            {
                return NotFound();
            }

            return View(artistModel);
        }

        // POST: Artist/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var artistModel = await _context.Artists.SingleOrDefaultAsync(m => m.Id == id);
            _context.Artists.Remove(artistModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public IActionResult FollowOrUnfollow(long? id)
        {
            var user = _context.Users.Single(x => x.UserName == User.Identity.Name);
            var artist = _context.Artists.Single(x => x.Id == id);

            var userFollow = new UserFollowModel();

            userFollow.User = user;
            userFollow.Artist = artist;

            if (_context.UserFollows
                .Where(x => x.ArtistId == id && x.UserId == user.Id)
                .Count() == 0)
            {
                try
                {
                    _context.Add(userFollow);
                    _context.SaveChanges();
                }
                catch { }
            }
            else
            {
                _context.Remove(userFollow);
                _context.SaveChanges();
            }


            return Redirect("/Artist/Details/" + id);
        }

        private bool ArtistModelExists(long id)
        {
            return _context.Artists.Any(e => e.Id == id);
        }
    }
}

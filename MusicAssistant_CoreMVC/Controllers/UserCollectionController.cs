using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicAssistantMvcCore.Models;
using MusicAssistant_CoreMVC.Data;

namespace MusicAssistant_CoreMVC.Controllers
{
    public class UserCollectionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserCollectionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserCollection
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.UserCollections.Include(u => u.Song).Include(u => u.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: UserCollection/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCollectionModel = await _context.UserCollections
                .Include(u => u.Song)
                .Include(u => u.User)
                .SingleOrDefaultAsync(m => m.SongId == id);
            if (userCollectionModel == null)
            {
                return NotFound();
            }

            return View(userCollectionModel);
        }

        // GET: UserCollection/Create
        public IActionResult Create()
        {
            ViewData["SongId"] = new SelectList(_context.Songs, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: UserCollection/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SongId,UserId")] UserCollectionModel userCollectionModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userCollectionModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SongId"] = new SelectList(_context.Songs, "Id", "Id", userCollectionModel.SongId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userCollectionModel.UserId);
            return View(userCollectionModel);
        }

        // GET: UserCollection/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCollectionModel = await _context.UserCollections.SingleOrDefaultAsync(m => m.SongId == id);
            if (userCollectionModel == null)
            {
                return NotFound();
            }
            ViewData["SongId"] = new SelectList(_context.Songs, "Id", "Id", userCollectionModel.SongId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userCollectionModel.UserId);
            return View(userCollectionModel);
        }

        // POST: UserCollection/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("SongId,UserId")] UserCollectionModel userCollectionModel)
        {
            if (id != userCollectionModel.SongId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userCollectionModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserCollectionModelExists(userCollectionModel.SongId))
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
            ViewData["SongId"] = new SelectList(_context.Songs, "Id", "Id", userCollectionModel.SongId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userCollectionModel.UserId);
            return View(userCollectionModel);
        }

        // GET: UserCollection/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCollectionModel = await _context.UserCollections
                .Include(u => u.Song)
                .Include(u => u.User)
                .SingleOrDefaultAsync(m => m.SongId == id);
            if (userCollectionModel == null)
            {
                return NotFound();
            }

            return View(userCollectionModel);
        }

        // POST: UserCollection/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var userCollectionModel = await _context.UserCollections.SingleOrDefaultAsync(m => m.SongId == id);
            _context.UserCollections.Remove(userCollectionModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserCollectionModelExists(long id)
        {
            return _context.UserCollections.Any(e => e.SongId == id);
        }
    }
}

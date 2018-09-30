using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicAssistantMvcCore.Models;
using MusicAssistant_CoreMVC.Data;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace MusicAssistant_CoreMVC.Controllers
{
    [Authorize(Roles = "Moderator")]
    public class SongController : Controller
    {
        private readonly ApplicationDbContext _context;

        public struct AlbumItem
        {
            int Id;
            string Name;
        }

        public SongController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Song
        public IActionResult Index()
        {
            var songsList = _context.Songs.ToList();
            foreach (var song in songsList)
            {
                _context.Entry(song).Reference(x => x.Album).Load();
            }

            ViewBag.Artists = new SelectList(_context.Artists, "Id", "Name");
            ViewBag.Albums = new SelectList(_context.Albums, "Id", "Name");

            return View(songsList);
        }

        public IActionResult Filter(int artist, int album)
        {
            var songsList = _context.Songs.ToList();
            foreach (var song in songsList)
            {
                _context.Entry(song).Reference(x => x.Album).Load();
            }

            if (artist != 0)    // if we are filtering by artist
            {
                var artistAlbums = _context.Artists.Single(x => x.Id == artist);
                _context.Entry(artistAlbums).Collection(x => x.Album).Load();
                var fittingAlbums = artistAlbums.Album;
                songsList = songsList.Where(x => fittingAlbums.Contains(x.Album)).ToList();
                ViewBag.Albums = new SelectList(_context.Albums.Where(x => x.Artist.Id == artist), "Id", "Name");
            }
            else                // if we are filtering by album
            {
                songsList = songsList.Where(x => x.Album.Id == album).ToList();
                ViewBag.Albums = new SelectList(_context.Albums, "Id", "Name");
            }

            ViewBag.Artists = new SelectList(_context.Artists, "Id", "Name");

            return View("Index", songsList);
        }

        // GET: Song/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var songModel = await _context.Songs
                .SingleOrDefaultAsync(m => m.Id == id);
            if (songModel == null)
            {
                return NotFound();
            }

            _context.Entry(songModel).Reference(x => x.Album).Load();
            return View(songModel);
        }

        // GET: Song/Create
        public IActionResult Create()
        {
            var p = new SongViewModel();
            p.AlbumsList = _context.Albums.ToList();
            return View(p);
        }

        // POST: Song/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Album, Name, SongText")] SongModel songModel)
        {
            if (ModelState.IsValid)
            {
                songModel.Album = _context.Albums.Single(x => x.Id == songModel.Album.Id);
                _context.Add(songModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(songModel);
        }

        // GET: Song/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var songModel = await _context.Songs.SingleOrDefaultAsync(m => m.Id == id);
            if (songModel == null)
            {
                return NotFound();
            }

            _context.Entry(songModel).Reference(x => x.Album).Load();

            var p = new SongViewModel();
            p.Id = songModel.Id;
            p.Name = songModel.Name;
            p.SongText = songModel.SongText;
            p.Album = songModel.Album;
            p.AlbumsList = _context.Albums.ToList();

            return View(p);
        }

        // POST: Song/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Album,SongText")] SongModel songModel)
        {
            if (id != songModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    songModel.Album = _context.Albums.Single(x => x.Id == songModel.Album.Id);
                    _context.Update(songModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SongModelExists(songModel.Id))
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
            return View(songModel);
        }

        // GET: Song/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var songModel = await _context.Songs
                .SingleOrDefaultAsync(m => m.Id == id);
            if (songModel == null)
            {
                return NotFound();
            }

            return View(songModel);
        }

        // POST: Song/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var songModel = await _context.Songs.SingleOrDefaultAsync(m => m.Id == id);
            _context.Songs.Remove(songModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SongModelExists(long id)
        {
            return _context.Songs.Any(e => e.Id == id);
        }
    }
}

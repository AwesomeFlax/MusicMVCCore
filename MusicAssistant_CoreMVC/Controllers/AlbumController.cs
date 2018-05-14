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
using Microsoft.AspNetCore.Identity;
using MusicAssistant_CoreMVC.Models;

namespace MusicAssistant_CoreMVC.Controllers
{
    public class AlbumController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AlbumController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Album
        public IActionResult Index()
        {
            var albumsList = _context.Albums.ToList();
            foreach (var album in albumsList)
            {
                _context.Entry(album).Reference(x => x.Artist).Load();
            }

            return View(albumsList);
        }

        // GET: Album/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var albumModel = await _context.Albums.SingleOrDefaultAsync(m => m.Id == id);
            _context.Entry(albumModel).Collection(x => x.Song).Load();

            var p = new AlbumViewModel();
            p.Id = albumModel.Id;
            p.Name = albumModel.Name;
            p.Created = albumModel.Created;
            p.Genre = albumModel.Genre;
            p.Artist = albumModel.Artist;
            p.Description = albumModel.Description;
            p.Song = albumModel.Song;
            p.AlbumPhotoUrl = albumModel.AlbumPhotoUrl;
            p.ArtistsList = _context.Artists.ToList();

            if (User.Identity.Name != null)
            {
                var user = _context.Users.Single(x => x.UserName == User.Identity.Name);
                p.UserCollections = _context.UserCollections.Where(x => x.UserId == user.Id).ToList();
            }

            if (p == null)
            {
                return NotFound();
            }

            return View(p);
        }

        // GET: Album/Create
        [Authorize(Roles = "Moderator")]
        public IActionResult Create()
        {
            var p = new AlbumViewModel();
            p.ArtistsList = _context.Artists.ToList();
            return View(p);
        }

        // POST: Album/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Genre,Artist,Created,Description,AlbumPhotoUrl")] AlbumModel albumModel)
        {
            if (ModelState.IsValid)
            {
                albumModel.Artist = _context.Artists.Single(x => x.Id == albumModel.Artist.Id);
                _context.Add(albumModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(albumModel);
        }

        // GET: Album/Edit/5
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var albumModel = await _context.Albums.SingleOrDefaultAsync(m => m.Id == id);
            if (albumModel == null)
            {
                return NotFound();
            }

            _context.Entry(albumModel).Reference(x => x.Artist).Load();

            var p = new AlbumViewModel();
            p.Id = albumModel.Id;
            p.Name = albumModel.Name;
            p.Created = albumModel.Created;
            p.Genre = albumModel.Genre;
            p.Artist = albumModel.Artist;
            p.Description = albumModel.Description;
            p.ArtistsList = _context.Artists.ToList();
            p.AlbumPhotoUrl = albumModel.AlbumPhotoUrl;

            return View(p);
        }

        // POST: Album/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Genre,Artist,Created,Description,AlbumPhotoUrl")] AlbumModel albumModel)
        {
            if (id != albumModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    albumModel.Artist = _context.Artists.Single(x => x.Id == albumModel.Artist.Id);
                    _context.Update(albumModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumModelExists(albumModel.Id))
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
            return View(albumModel);
        }

        // GET: Album/Delete/5
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var albumModel = await _context.Albums
                .SingleOrDefaultAsync(m => m.Id == id);
            if (albumModel == null)
            {
                return NotFound();
            }

            return View(albumModel);
        }

        // POST: Album/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var albumModel = await _context.Albums.SingleOrDefaultAsync(m => m.Id == id);
            _context.Albums.Remove(albumModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlbumModelExists(long id)
        {
            return _context.Albums.Any(e => e.Id == id);
        }

        [Authorize]
        public IActionResult AddOrRemoveCollection(long? id)
        {
            var userCollectionModel = new UserCollectionModel();

            var user = _context.Users.Single(x => x.UserName == User.Identity.Name);
            userCollectionModel.User = user;
            userCollectionModel.Song = _context.Songs.Single(x => x.Id == id);

            if (_context.UserCollections
                .Where(x => x.SongId == id && x.UserId == user.Id)
                .Count() == 0)
            {
                _context.Add(userCollectionModel);
                _context.SaveChanges();
            }
            else
            {
                _context.Remove(userCollectionModel);
                _context.SaveChanges();
            }

            _context.Entry(userCollectionModel.Song).Reference(x => x.Album).Load();
            var album = _context.Albums.Single(x => userCollectionModel.Song.Album.Id == x.Id);

            return Redirect("/Album/Details/" + album.Id);
        }

        [Authorize]
        public IActionResult AddAlbumToCollection(long? id)
        {
            var user = _context.Users.Single(x => x.UserName == User.Identity.Name);
            var album = _context.Albums.Single(x => x.Id == id);
            _context.Entry(album).Collection(x => x.Song).Load();

            foreach (var song in album.Song)
            {
                var userCollectionModel = new UserCollectionModel();

                userCollectionModel.User = user;
                userCollectionModel.Song = song;

                if (_context.UserCollections
                    .Where(x => x.SongId == id && x.UserId == user.Id)
                    .Count() == 0)
                {
                    try
                    {
                        _context.Add(userCollectionModel);
                        _context.SaveChanges();
                    }
                    catch { }
                }
            }

            return Redirect("/Album/Details/" + id);
        }
    }
}

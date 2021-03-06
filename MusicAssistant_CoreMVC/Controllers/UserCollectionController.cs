﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicAssistantMvcCore.Models;
using MusicAssistant_CoreMVC.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace MusicAssistant_CoreMVC.Controllers
{
    [Authorize]
    public class UserCollectionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserCollectionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserCollection
        public IActionResult Index()
        {
            var applicationDbContext = _context.UserCollections.Include(u => u.Song).Include(u => u.User);
            var collectionsList = applicationDbContext.Where(x => x.User.UserName == User.Identity.Name);
            List<AlbumModel> albums = new List<AlbumModel>();

            foreach (var collection in collectionsList)
            {
                _context.Entry(collection.Song).Reference(x => x.Album).Load();
                var album = collection.Song.Album;
                if (!albums.Contains(album))
                {
                    albums.Add(album);
                }
            }

            return View(albums);
        }

        // GET: Album/Details/5
        public IActionResult Details(long? id)
        {
            if (id != null)
            {
                return Redirect("/Album/Details/" + id);
            }
            else
            {
                return NotFound();
            }
        }
    }
}

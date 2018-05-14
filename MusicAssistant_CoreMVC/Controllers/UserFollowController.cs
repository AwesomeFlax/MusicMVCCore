using System;
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
    public class UserFollowController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserFollowController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserCollection
        public IActionResult Index()
        {
            var applicationDbContext = _context.UserFollows.Include(u => u.Artist).Include(u => u.User);
            var followsList = applicationDbContext.Where(x => x.User.UserName == User.Identity.Name);
            List<ArtistModel> artists = new List<ArtistModel>();

            foreach (var follow in followsList)
            {
                artists.Add(follow.Artist);
            }

            return View(artists);
        }

        // GET: Album/Details/5
        public IActionResult Details(long? id)
        {
            if (id != null)
            {
                return Redirect("/Artist/Details/" + id);
            }
            else
            {
                return NotFound();
            }
        }
    }
}

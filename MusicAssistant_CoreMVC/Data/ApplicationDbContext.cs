using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusicAssistant_CoreMVC.Models;
using MusicAssistantMvcCore.Models;

namespace MusicAssistant_CoreMVC.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AlbumModel> Albums { get; set; }
        public DbSet<ArtistModel> Artists { get; set; }
        public DbSet<RewardModel> Rewards { get; set; }
        public DbSet<SongModel> Songs { get; set; }
        public DbSet<UserCollectionModel> UserCollections { get; set; }
        public DbSet<UserFollowModel> UserFollows { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AlbumModel>().ToTable("Albums");
            builder.Entity<ArtistModel>().ToTable("Artists");
            builder.Entity<RewardModel>().ToTable("Rewards");
            builder.Entity<SongModel>().ToTable("Songs");
            builder.Entity<UserCollectionModel>().ToTable("UserCollections");
            builder.Entity<UserCollectionModel>().HasKey(u => new { u.SongId, u.UserId });
            builder.Entity<UserFollowModel>().ToTable("UserFollows");
            builder.Entity<UserFollowModel>().HasKey(u => new { u.ArtistId, u.UserId });
        }
    }
}

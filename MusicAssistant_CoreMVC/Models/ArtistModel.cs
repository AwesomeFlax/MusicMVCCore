using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MusicAssistantMvcCore.Models
{
    public class ArtistModel
    {
        [Key] public long Id { get; set; }
        [MaxLength(50)] public string Pseudonym { get; set; }
        [MaxLength(50)] public string Name { get; set; }
        [MaxLength(50)] public string LastName { get; set; }
        [Column(TypeName = "DateTime2")] public DateTime? BirthDate { get; set; }
        [Column(TypeName = "DateTime2")] public DateTime? DeathDate { get; set; }
        [Column(TypeName = "DateTime2")] public DateTime? CareerStart { get; set; }
        [Column(TypeName = "DateTime2")] public DateTime? CareerEnd { get; set; }
        [MaxLength(80)] public string BirthPlace { get; set; }
        public string Gender { get; set; }
        public string Biography { get; set; }
        public string ArtistPhotoUrl { get; set; }

        public ICollection<RewardModel> Reward { get; set; }
        public ICollection<AlbumModel> Album { get; set; }
        public ICollection<UserFollowModel> UserFollow { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MusicAssistantMvcCore.Models
{
    public class AlbumModel
    {
       [Key] public long Id { get; set; }
       public ArtistModel Artist { get; set; }
       [MaxLength(50)] public string Name { get; set; }
       public string Genre { get; set; }
       [Column(TypeName = "DateTime2")] public DateTime Created { get; set; }
       [MaxLength(400)] public string Description { get; set; }
       public string AlbumPhotoUrl { get; set; }

       public ICollection<SongModel> Song { get; set; }
    }
}

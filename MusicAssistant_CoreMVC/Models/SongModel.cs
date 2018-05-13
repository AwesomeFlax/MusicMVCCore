using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MusicAssistantMvcCore.Models
{
    public class SongModel
    {
        [Key] public long Id { get; set; }
        public AlbumModel Album { get; set; }
        [MaxLength(50)] public string Name { get; set; }
        public string SongText { get; set; }

        public ICollection<UserCollectionModel> UserCollection { get; set; }
    }

    public class SongViewModel : SongModel
    {
        public List<AlbumModel>AlbumsList { get; set; }
    }
}

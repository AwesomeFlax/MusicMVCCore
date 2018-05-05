using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicAssistantMvcCore.Models
{
    public class RewardModel
    {
        [Key] public long Id { get; set; }
        public ArtistModel Artist { get; set; }
        [MaxLength(50)] public string Name { get; set; }
        [MaxLength(1000)] public string Description { get; set; }
        public DateTime HandingDate { get; set; }
    }
}

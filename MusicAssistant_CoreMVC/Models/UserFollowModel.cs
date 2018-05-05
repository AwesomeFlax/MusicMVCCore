using Microsoft.AspNetCore.DataProtection.KeyManagement;
using MusicAssistant_CoreMVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MusicAssistantMvcCore.Models
{
    public class UserFollowModel
    {
        [Key, Column(Order = 0)] public long ArtistId { get; set; }
        [Key, Column(Order = 1)] public string UserId { get; set; }

        [ForeignKey("ArtistId")] public virtual ArtistModel Artist { get; set; }
        [ForeignKey("UserId")] public virtual ApplicationUser User { get; set; }
    }
}

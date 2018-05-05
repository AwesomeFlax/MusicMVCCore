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
    public class UserCollectionModel
    {
        [Key, Column(Order = 0)] public long SongId { get; set; }
        [Key, Column(Order = 1)] public string UserId { get; set; }

        [ForeignKey("SongId")] public virtual SongModel Song { get; set; }
        [ForeignKey("UserId")] public virtual ApplicationUser User { get; set; }
    }
}

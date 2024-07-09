using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EichkustMusic.Tracks.Domain.Entities
{
    public class PlaylistTrack
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int TrackId { get; set; }

        public int PlaylistId { get; set; }

        public Track Track { get; set; } = null!;

        public Playlist Playlist { get; set; } = null!;
    }
}

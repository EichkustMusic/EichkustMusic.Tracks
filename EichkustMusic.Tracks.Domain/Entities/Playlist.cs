using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EichkustMusic.Tracks.Domain.Entities
{
    public class Playlist
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(64)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(1024)]
        public string Description { get; set; } = null!;

        public ICollection<PlaylistTrack> PlaylistTracks { get; set; } = new List<PlaylistTrack>();

        public IEnumerable<Track> Tracks
        {
            get
            {
                return PlaylistTracks.Select(pt => pt.Track);
            }
        }
    }
}

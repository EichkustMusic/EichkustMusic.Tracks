using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EichkustMusic.Tracks.Application.DTOs.Playlist
{
    public class PlaylistForCreateDTO
    {
        [Required]
        [MaxLength(64)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(1024)]
        public string Description { get; set; } = null!;

        public IEnumerable<int> TracksIds { get; set; } = new List<int>();

        public Domain.Entities.Playlist MapToPlaylist()
        {
            return new Domain.Entities.Playlist
            {
                Name = Name,
                Description = Description
            };
        }
    }
}

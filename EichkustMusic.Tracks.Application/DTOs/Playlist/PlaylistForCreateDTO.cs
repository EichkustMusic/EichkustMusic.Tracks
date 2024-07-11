using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EichkustMusic.Tracks.Application.DTOs.Playlist
{
    public class PlaylistForCreateDTO
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public IEnumerable<int> TracksIds { get; set; } = new List<int>();
    }
}

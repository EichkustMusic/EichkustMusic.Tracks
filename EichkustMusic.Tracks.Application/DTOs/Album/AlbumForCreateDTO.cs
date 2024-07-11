using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EichkustMusic.Tracks.Application.DTOs.Album
{
    public class AlbumForCreateDTO
    {
        public string? Description { get; set; }

        public ICollection<int> TracksIds { get; set; } = new List<int>();
    }
}

using EichkustMusic.Tracks.Application.DTOs.Track;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EichkustMusic.Tracks.Application.DTOs.Album
{
    public class AlbumCreateResultDTO
    {
        public int Id { get; set; }

        public string? Description { get; set; }

        public IEnumerable<TrackDTO> Tracks { get ; set; } = new List<TrackDTO>();
        
        public string PathToUploadCoverImage { get; set; } = null!;
    }
}

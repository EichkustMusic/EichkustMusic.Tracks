using EichkustMusic.Tracks.Application.DTOs.Album;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EichkustMusic.Tracks.Application.DTOs.Track
{
    public class TrackDTO
    {
        public int Id { get; set; }

        public string? Description { get; set; }

        public int UserId { get; set; }

        public string CoverImagePath { get; set; } = null!;

        public string MusicPath { get; set; } = null!;

        public AlbumDTO? Album { get; set; }
    }
}

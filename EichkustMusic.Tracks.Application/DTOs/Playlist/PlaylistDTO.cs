using EichkustMusic.Tracks.Application.DTOs.Track;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EichkustMusic.Tracks.Application.DTOs.Playlist
{
    public class PlaylistDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public IEnumerable<TrackDTO> Tracks { get; set; } = new List<TrackDTO>();

        public static PlaylistDTO MapFromPlaylist(Domain.Entities.Playlist playlist)
        {
            return new PlaylistDTO
            {
                Id = playlist.Id,
                Name = playlist.Name,
                Description = playlist.Description,
                UserId = playlist.UserId,
                Tracks = TrackDTO.MapFromTracksListToTrackDTOsList(playlist.Tracks),
            };
        }
    }
}

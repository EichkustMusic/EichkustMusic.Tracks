using EichkustMusic.Tracks.Application.DTOs.Track;
using EichkustMusic.Tracks.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EichkustMusic.Tracks.Application.DTOs.Album
{
    public class AlbumDTO
    {
        public int Id { get; set; }

        public string? Description { get; set; }

        public IEnumerable<TrackDTO> Tracks { get; set; } = new List<TrackDTO>();

        public static AlbumDTO MapFromAlbum(Domain.Entities.Album album)
        {
            return new AlbumDTO
            {
                Id = album.Id,
                Description = album.Description,
                Tracks = TrackDTO.MapFromTracksListToTrackDTOsList(
                    (List<Domain.Entities.Track>)album.Tracks)
            };
        }
    }
}

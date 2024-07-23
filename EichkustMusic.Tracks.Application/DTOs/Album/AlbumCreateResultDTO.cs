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

        public static AlbumCreateResultDTO MapFromAlbum(Domain.Entities.Album album)
        {
            return new AlbumCreateResultDTO
            {
                Id = album.Id,
                Description = album.Description,
                Tracks = 
                    album.Tracks != null
                    ? TrackDTO.MapFromTracksListToTrackDTOsList(album.Tracks)
                    : []
            };
        }
    }
}

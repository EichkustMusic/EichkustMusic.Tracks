using EichkustMusic.Tracks.Application.DTOs.Album;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EichkustMusic.Tracks.Application.DTOs.Track
{
    public class TrackCreateResultDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int UserId { get; set; }

        public AlbumDTO? Album { get; set; }

        public string PathToUploadCoverImage { get; set; } = null!;

        public string PathToUploadMusic { get; set; } = null!;

        public static TrackCreateResultDTO MapFromTrack(Domain.Entities.Track track)
        {
            return new TrackCreateResultDTO
            {
                Id = track.Id,
                Description = track.Description,
                UserId = track.UserId,
                Album = 
                    track.Album == null
                    ? null
                    : AlbumDTO.MapFromAlbum(track.Album),
            };
        }
    }
}

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

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int UserId { get; set; }

        public string? CoverImagePath { get; set; }

        public string? MusicPath { get; set; }

        public AlbumDTO? Album { get; set; }

        public static TrackDTO MapFromTrack(Domain.Entities.Track track, bool withAlbum = true)
        {
            return new TrackDTO
            {
                Id = track.Id,
                Name = track.Name,
                Description = track.Description,
                UserId = track.UserId,
                CoverImagePath = track.CoverImagePath,
                MusicPath = track.MusicPath,
                Album = 
                    track.Album == null || withAlbum == false
                    ? null
                    : AlbumDTO.MapFromAlbum(track.Album)
            };
        }

        public static List<TrackDTO> MapFromTracksListToTrackDTOsList(
            IEnumerable<Domain.Entities.Track> tracks, bool withAlbums = false)
        {
            var trackDTOs = new List<TrackDTO>();

            foreach (var track in tracks)
            {
                trackDTOs.Add(MapFromTrack(track, withAlbum: withAlbums));
            }

            return trackDTOs;
        }
    }
}

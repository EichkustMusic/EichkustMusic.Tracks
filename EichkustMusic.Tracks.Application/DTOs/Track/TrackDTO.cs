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

        public string? CoverImagePath { get; set; }

        public string? MusicPath { get; set; }

        public AlbumDTO? Album { get; set; }

        public static TrackDTO MapFromTrack(Domain.Entities.Track track)
        {
            return new TrackDTO
            {
                Id = track.Id,
                Description = track.Description,
                UserId = track.UserId,
                CoverImagePath = track.CoverImagePath,
                MusicPath = track.MusicPath,
                Album = 
                    track.Album is null
                    ? null
                    : AlbumDTO.MapFromAlbum(track.Album)
            };
        }

        public static List<TrackDTO> MapFromTracksListToTrackDTOsList(
            List<Domain.Entities.Track> tracks)
        {
            var trackDTOs = new List<TrackDTO>();

            tracks.ForEach(t =>
                trackDTOs.Add(
                    MapFromTrack(t)));

            return trackDTOs;
        }
    }
}

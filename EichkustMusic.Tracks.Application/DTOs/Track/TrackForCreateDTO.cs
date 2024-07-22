using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EichkustMusic.Tracks.Application.DTOs.Track
{
    public class TrackForCreateDTO
    {
        [MaxLength(2048)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(64)]
        public string Name { get; set; } = null!;

        [Required]
        public int UserId { get; set; }

        public int? AlbumId { get; set; }

        public string? CoverImagePath { get; set; }

        public Domain.Entities.Track MapToTrack()
        {
            return new Domain.Entities.Track
            {
                Description = Description,
                Name = Name,
                UserId = UserId,
                AlbumId = AlbumId,
                CoverImagePath = CoverImagePath,
            };
        }
    }
}

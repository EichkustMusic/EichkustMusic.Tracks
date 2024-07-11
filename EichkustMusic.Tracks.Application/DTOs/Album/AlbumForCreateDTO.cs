using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EichkustMusic.Tracks.Application.DTOs.Album
{
    public class AlbumForCreateDTO
    {
        [Required]
        public string Name { get; set; } = null!;

        [MaxLength(1024)]
        public string? Description { get; set; }

        public ICollection<int> TracksIds { get; set; } = new List<int>();

        public Domain.Entities.Album MapToAlbum()
        {
            return new Domain.Entities.Album
            {
                Name = Name,
                Description = Description
            };
        }
    }
}

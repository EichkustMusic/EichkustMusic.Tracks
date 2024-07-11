using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EichkustMusic.Tracks.Domain.Entities
{
    public class Track
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(64)]
        public string Name { get; set; } = null!;

        [MaxLength(2048)]
        public string? Description { get; set; }

        [Required]
        public int UserId { get; set; }

        public string? CoverImagePath { get; set; }

        public string? MusicPath { get; set; }

        public int? AlbumId { get; set; }

        public Album? Album { get; set; }
    }
}

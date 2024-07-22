using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EichkustMusic.Tracks.Domain.Entities
{
    public class Album
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(64)]
        public string Name { get; set; } = null!;

        [MaxLength(1024)]
        public string? Description { get; set; }

        public string? CoverImagePath { get; set; }

        public ICollection<Track> Tracks { get; set; } = new List<Track>();
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EichkustMusic.Tracks.Application.DTOs.Track
{
    public class TrackForCreateDTO
    {
        public string? Description { get; set; }

        public int UserId { get; set; }

        public int? AlbumId { get; set; }
    }
}

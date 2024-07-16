using EichkustMusic.Tracks.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EichkustMusic.Tracks.Testing.InfrastructureTests.UnitOfWorkTests.Mocks
{
    internal static class TrackDbSetMock
    {
        public static List<Track> Mock { get; set; } =
        [
            new()
            {
                Id = 1,
                Name = "First_name",
                Description = "Second_description",
                UserId = 1,
                CoverImagePath = "path",
                MusicPath = null,
                AlbumId = 1,
                Album = null
            },
            new()
            {
                Id = 2,
                Name = "Second_name",
                Description = "First_description",
                UserId = 1,
                CoverImagePath = "path",
                MusicPath = null,
                AlbumId = 1,
                Album = null
            },
            new()
            {
                Id = 3,
                Name = "Third_name",
                Description = "third_description",
                UserId = 1,
                CoverImagePath = "path",
                MusicPath = null,
                AlbumId = 1,
                Album = null
            },
        ];

    }
}

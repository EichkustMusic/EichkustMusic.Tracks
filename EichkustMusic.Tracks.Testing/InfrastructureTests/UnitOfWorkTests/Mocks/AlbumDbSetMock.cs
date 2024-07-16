using EichkustMusic.Tracks.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EichkustMusic.Tracks.Testing.InfrastructureTests.UnitOfWorkTests.Mocks
{
    public static class AlbumDbSetMock
    {
        public static List<Album> Albums { get; set; } =
        [
            new()
            {
                Id = 1,
                Name = "First_name",
                Description = "Second_description",
                Tracks = []
            },
            new()
            {
                Id = 2,
                Name = "Second_name",
                Description = "First_description",
                Tracks = []
            },
            new()
            {
                Id = 3,
                Name = "Third_name",
                Description = "Third_description",
                Tracks = []
            }
        ];
    }
}

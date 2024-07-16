using EichkustMusic.Tracks.Application.UnitOfWork.Repositories;
using EichkustMusic.Tracks.Domain.Entities;
using EichkustMusic.Tracks.Infrastructure.Persistence;
using EichkustMusic.Tracks.Infrastructure.Persistence.UnitOfWork.Repositories;
using EichkustMusic.Tracks.Testing.InfrastructureTests.UnitOfWorkTests.Mocks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EichkustMusic.Tracks.Testing.InfrastructureTests.UnitOfWorkTests
{
    internal class AlbumRepositoryTests
    {
        private readonly IAlbumRepository _albumRepository;

        public AlbumRepositoryTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<TracksDbContext>()
                .UseInMemoryDatabase("TracksDb")
                .Options;

            var dbContext = new TracksDbContext(dbContextOptions);

            dbContext.AddRange(AlbumDbSetMock.Albums);
            dbContext.SaveChanges();

            _albumRepository = new AlbumRepository(dbContext);
        }

        [Test]
        public async Task AlbumRepository_ListAsync_SearchesCorrect()
        {
            var actual = await _albumRepository
                .ListAsync(1, 3, "First_");

            var expected = new List<Album>()
            {
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
            };

            Assert.That(
                JsonSerializer.Serialize(actual),
                Is.EqualTo(JsonSerializer.Serialize(expected)));
        }

        [Test]
        public async Task AlbumRepository_ListAsync_DontSeeRegister()
        {
            var actual = await _albumRepository
                .ListAsync(1, 2, "first_");

            var expected = new List<Album>()
            {
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
            };

            Assert.That(
                JsonSerializer.Serialize(actual),
                Is.EqualTo(JsonSerializer.Serialize(expected)));
        }
    }
}

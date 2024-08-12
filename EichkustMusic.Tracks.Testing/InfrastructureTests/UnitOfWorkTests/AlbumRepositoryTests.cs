using EichkustMusic.S3;
using EichkustMusic.Tracks.Application.UnitOfWork.Repositories;
using EichkustMusic.Tracks.Domain.Entities;
using EichkustMusic.Tracks.Infrastructure.Persistence;
using EichkustMusic.Tracks.Infrastructure.Persistence.UnitOfWork.Repositories;
using EichkustMusic.Tracks.Testing.InfrastructureTests.UnitOfWorkTests.Mocks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Diagnostics;
using System.Text.Json;

namespace EichkustMusic.Tracks.Testing.InfrastructureTests.UnitOfWorkTests
{
    internal class AlbumRepositoryTests
    {
        private readonly IAlbumRepository _albumRepository;

        public AlbumRepositoryTests()
        {
            // Configure S3
            // .env file should be created at bin/Env/net8.0/
            var directory = $"{Directory.GetCurrentDirectory()}" + "/.env";

            Debug.WriteLine(directory);

            DotNetEnv.Env.Load(directory);

            var configurationManagerMock = new Mock<IConfigurationManager>();

            var accessKey = Environment.GetEnvironmentVariable("S3_ACCESS_KEY");
            var secretKey = Environment.GetEnvironmentVariable("S3_SECRET_KEY");
            var serviceUrl = Environment.GetEnvironmentVariable("S3_SERVICE_URL");

            configurationManagerMock
                .Setup(cm => cm["S3:AccessKey"])
                .Returns(accessKey);

            configurationManagerMock
                .Setup(cm => cm["S3:SecretKey"])
                .Returns(secretKey);

            configurationManagerMock
                .Setup(cm => cm["S3:ServiceUrl"])
                .Returns(secretKey);

            var s3 = new S3Storage(configurationManagerMock.Object);

            // Configure DbContext
            var dbContextOptions = new DbContextOptionsBuilder<TracksDbContext>()
                .UseInMemoryDatabase("TracksDb")
                .Options;

            var dbContext = new TracksDbContext(dbContextOptions);

            dbContext.AddRange(AlbumDbSetMock.Albums);
            dbContext.SaveChanges();

            // Configure repository
            _albumRepository = new AlbumRepository(dbContext ,s3);
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

using EichkustMusic.Tracks.Infrastructure.Persistence;
using Moq;
using EichkustMusic.Tracks.Application.UnitOfWork.Repositories;
using EichkustMusic.Tracks.Infrastructure.Persistence.UnitOfWork.Repositories;
using EichkustMusic.Tracks.Testing.InfrastructureTests.UnitOfWorkTests.Mocks;
using Microsoft.EntityFrameworkCore;
using EichkustMusic.Tracks.Domain.Entities;
using System.Text.Json;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using EichkustMusic.Tracks.Application.UnitOfWork.Exceptions;
using EichkustMusic.S3;

namespace EichkustMusic.Tracks.Testing.InfrastructureTests.UnitOfWorkTests
{
    public class TrackRepositoryTests
    {
        private readonly ITrackRepository _trackRepository;
        private readonly TracksDbContext _dbContext;
        private readonly IS3Storage _s3;

        public TrackRepositoryTests()
        {
            // Configure S3
            // .env file should be created at bin/Env/net8.0/
            var directory = $"{Directory.GetCurrentDirectory()}" + "/.env";

            Debug.WriteLine(directory);

            DotNetEnv.Env.Load(directory);

            var configurationManagerMock = new Mock<IConfigurationManager>();

            var accessKey = Environment.GetEnvironmentVariable("S3_ACCESS_KEY");
            var secretKey = Environment.GetEnvironmentVariable("S3_SECRET_KEY");

            configurationManagerMock
                .Setup(cm => cm["S3:AccessKey"])
                .Returns(accessKey);

            configurationManagerMock
                .Setup(cm => cm["S3:SecretKey"])
                .Returns(secretKey);

            var s3 = new S3Storage(configurationManagerMock.Object);

            _s3 = s3;

            // Configure DbContext
            var dbContextOptions = new DbContextOptionsBuilder<TracksDbContext>()
                .UseInMemoryDatabase("TracksDb")
                .Options;

            var mockDbContext = new TracksDbContext(dbContextOptions);

            mockDbContext.AddRange(TrackDbSetMock.Mock);

            mockDbContext.SaveChanges();

            _dbContext = mockDbContext;

            // Configure repository
            _trackRepository = new TrackRepository(mockDbContext, s3);
        }

        [Test]
        public async Task TrackRepository_GetByIdAsync_SearchesIfExist()
        {
            Assert.IsNotNull(await _trackRepository.GetByIdAsync(1));
        }

        [Test]
        public async Task TrackRepository_GetByIdAsync_ReturnsNullIfNotExist()
        {
            Assert.IsNull(await _trackRepository.GetByIdAsync(1000));
        }

        [Test]
        public async Task TrackRepository_ListAsync_ReturnsCorrectItems()
        {
            var result = await _trackRepository.ListAsync(1, 2, null);

            Assert.That(TrackDbSetMock.Mock.Take(2), Is.EqualTo(result));
        }

        [Test]
        public async Task TrackRepository_ListAsync_SearchesCorrect()
        {
            var actual = await _trackRepository
                .ListAsync(1, 3, "First_");

            var expected = new List<Track>()
            {
                new Track()
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
                new Track()
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
            };

            Assert.That(
                JsonSerializer.Serialize(actual),
                Is.EqualTo(JsonSerializer.Serialize(expected)));
        }

        [Test]
        public async Task TrackRepository_ListAsync_DontSeeRegister()
        {
            var actual = await _trackRepository
                .ListAsync(1, 2, "first_");

            var expected = new List<Track>()
            {
                new Track()
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
                new Track()
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
            };

            Assert.That(
                JsonSerializer.Serialize(actual),
                Is.EqualTo(JsonSerializer.Serialize(expected)));
        }

        [Test]
        public async Task TrackRepository_DeleteAsync_DeletesMusicAndCoverFromS3()
        {
            var track = await _trackRepository.GetByIdAsync(4);

            if (track == null)
            {
                throw new Exception("Track not found");
            }

            var coverPath = track.CoverImagePath!;
            var musicPath = track.MusicPath!;

            await _trackRepository.DeleteAsync(track);

            await _dbContext.SaveChangesAsync();

            var doesCoverExist = await _s3.DoesFileExistAsync(coverPath);
            Assert.That(doesCoverExist, Is.False);

            var doesMusicExist = await _s3.DoesFileExistAsync(musicPath);
            Assert.That(doesMusicExist, Is.False);
        }

        [Test]
        public async Task TrackRepository_ApplyPatchDocumentAsyncTo_DeletesOldMusicFromS3WhenItChanges()
        {
            var track = await _trackRepository.GetByIdAsync(4);

            if (track == null)
            {
                throw new Exception("Track not found");
            }

            var oldMusicPath = track.MusicPath!;

            var patchDocument = new JsonPatchDocument();

            patchDocument.Operations.Add(new Operation()
            {
                op = "add",
                path = "/MusicPath",
                value = "https://s3.eu-west-2.wasabisys.com/eichkust-music-files/02.mp3"
            });
                
            await _trackRepository.ApplyPatchDocumentAsyncTo(track, patchDocument);

            await _dbContext.SaveChangesAsync();

            var doesOldCoverFileExist = await _s3.DoesFileExistAsync(oldMusicPath);

            Assert.That(doesOldCoverFileExist, Is.False);
        }

        [Test]
        public async Task TrackRepository_ApplyPatchDocumentAsyncTo_ThrowsIfNewMusicDoesNotExist()
        {
            var track = await _trackRepository.GetByIdAsync(4);

            if (track == null)
            {
                throw new Exception("Track not found");
            }

            var coverPath = track.CoverImagePath!;
            var musicPath = track.MusicPath!;

            var patchDocument = new JsonPatchDocument();

            patchDocument.Operations.Add(new Operation()
            {
                op = "add",
                path = "/MusicPath",
                value = "https://s3.eu-west-2.wasabisys.com/eichkust-music-files/03.mp3"
            });

            var oldCoverImagePath = track.CoverImagePath;

            await _dbContext.SaveChangesAsync();

            Assert.ThrowsAsync<NewFileNotFound>(async () =>
                await _trackRepository.ApplyPatchDocumentAsyncTo(track, patchDocument));
        }
    }
}

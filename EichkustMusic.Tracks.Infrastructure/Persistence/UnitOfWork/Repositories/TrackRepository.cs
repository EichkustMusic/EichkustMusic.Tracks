using EichkustMusic.Tracks.Application.S3;
using EichkustMusic.Tracks.Application.UnitOfWork.Exceptions;
using EichkustMusic.Tracks.Application.UnitOfWork.Repositories;
using EichkustMusic.Tracks.Domain.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EichkustMusic.Tracks.Infrastructure.Persistence.UnitOfWork.Repositories
{
    public class TrackRepository : ITrackRepository
    {
        private readonly TracksDbContext _dbContext;
        private readonly IS3Storage _s3;

        public TrackRepository(
            TracksDbContext dbContext, IS3Storage s3)
        {
            _dbContext = dbContext;
            _s3 = s3;
        }

        public void Add(Track track)
        {
            _dbContext.Add(track);
        }

        public async Task ApplyPatchDocumentAsyncTo(
            Track track, JsonPatchDocument patchDocument)
        {
            const string coverImagePath = "/coverimagepath";
            const string musicPath = "/musicpath";

            var s3Paths = new List<string>() {
                coverImagePath,
                musicPath
            };

            var s3Operations = patchDocument.Operations
                .Where(o => s3Paths.Contains(o.path.ToLower()));

            foreach (var s3Operation in s3Operations)
            {
                var filePath = (string)s3Operation.value;

                // Check if new file exist
                var doesFileExist = await _s3.DoesFileExistAsync(filePath);

                if (doesFileExist == false)
                {
                    throw new NewFileNotFound(filePath);
                }

                // Delete old files from S3
                if (
                    s3Operation.path.ToLower() == musicPath
                    && track.MusicPath != null)
                {
                    await _s3.DeleteFileAsync(track.MusicPath);
                }

                if (
                    s3Operation.path.ToLower() == coverImagePath
                    && track.CoverImagePath != null)
                {
                    await _s3.DeleteFileAsync(track.CoverImagePath);
                }
            }

            patchDocument.ApplyTo(track);
        }

        public async Task DeleteAsync(Track track)
        {
            if (track.CoverImagePath != null)
            {
                await _s3.DeleteFileAsync(track.CoverImagePath);
            }

            if (track.MusicPath != null)
            {
                await _s3.DeleteFileAsync(track.MusicPath);
            }

            _dbContext.Remove(track);
        }

        public async Task<Track?> GetByIdAsync(int id)
        {
            return await _dbContext.Tracks
                .Include(t => t.Album)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Track>> ListAsync(
            int pageNum, int pageSize, string? search)
        {
            IQueryable<Track> tracks = _dbContext.Tracks
                .Include(t => t.Album)
                .Select(t => new Track()
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    UserId = t.UserId,
                    CoverImagePath = t.CoverImagePath,
                    MusicPath = t.MusicPath,
                    AlbumId = t.AlbumId,
                    Album =
                        t.Album != null
                        ? new Album()
                        {
                            Id = t.Album.Id,
                            Name = t.Album.Name,
                            Description = t.Album.Description,
                            Tracks = new List<Track>()
                        }
                        : null
                });

            if (search != null)
            {
                tracks = tracks
                    .Where(t => 
                        t.Name
                            .ToLower()
                            .Contains(search.ToLower())
                        ||
                        (
                            t.Description != null
                            && t.Description
                                .ToLower()
                                .Contains(search.ToLower())));
            }

            return await tracks
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}

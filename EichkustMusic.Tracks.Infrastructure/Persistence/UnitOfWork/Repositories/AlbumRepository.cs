using EichkustMusic.Tracks.Application.S3;
using EichkustMusic.Tracks.Application.S3.Exceptions;
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
    public class AlbumRepository : IAlbumRepository
    {
        private readonly TracksDbContext _dbContext;
        private readonly IS3Storage _s3;

        public AlbumRepository(
            TracksDbContext dbContext, IS3Storage s3)
        {
            _dbContext = dbContext;
            _s3 = s3;
        }

        public async Task AddAsync(Album album)
        {
            _dbContext.Add(album);

            // Reset all tracks covers
            await SetAlbumTracksCoversAsync(album, null);
        }

        public async Task ApplyPatchDocumentAsyncTo(
            Album album, JsonPatchDocument patchDocument)
        {
            const string coverImagePath = "/coverimagepath";

            var s3Paths = new List<string>() {
                coverImagePath,
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
                    s3Operation.path.ToLower() == coverImagePath)
                {
                    if (album.CoverImagePath != null)
                    {
                        await _s3.DeleteFileAsync(album.CoverImagePath);
                    }

                    // Set new cover image to all of albums tracks
                    await SetAlbumTracksCoversAsync(album, album.CoverImagePath);
                    
                }
            }

            patchDocument.ApplyTo(album);
        }

        public async Task DeleteAsync(Album album)
        {
            if (album.CoverImagePath != null)
            {
                await _s3.DeleteFileAsync(album.CoverImagePath);
            }

            await SetAlbumTracksCoversAsync(album, null);

            _dbContext.Remove(album);
        }

        public async Task<Album?> GetByIdAsync(int id)
        {
            return await _dbContext.Albums
                .Include(a => a.Tracks)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Album>> ListAsync(
            int pageNum, int pageSize, string? search)
        {
            IQueryable<Album> albums = _dbContext.Albums
                .Include(a => a.Tracks);

            if (search != null)
            {
                albums = albums
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

            return await albums
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        /// <summary>
        /// Change track covers for all tracks of album.
        /// If <c>value</c> if <c>null</c> that old track covers are deleting from S3.
        /// </summary>
        /// <param name="album">Album for tracks for which you need to change track covers</param>
        /// <param name="value">New track cover</param>
        /// <returns></returns>
        /// <exception cref="S3FileDeleteException">Throws if deletion is unsuccessful</exception>
        internal async Task SetAlbumTracksCoversAsync(Album album, string? value)
        {
            foreach (var track in album.Tracks)
            {
                // Delete old tracks covers if new value is null (use for reset all tracks covers of album)
                if (track.CoverImagePath != null)
                {
                    if (await _s3.DeleteFileAsync(track.CoverImagePath) == true)
                    {
                        track.CoverImagePath = null;
                    }
                    else
                    {
                        throw new S3FileDeleteException($"Cannot delete cover image of track {track.Id}.");
                    }

                    return;
                }

                // Set new value if a file exist
                track.CoverImagePath = value;
            }
        }
    }
}

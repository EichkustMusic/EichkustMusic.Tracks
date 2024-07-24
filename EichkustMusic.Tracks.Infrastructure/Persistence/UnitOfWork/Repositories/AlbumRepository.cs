using EichkustMusic.Tracks.Application.S3;
using EichkustMusic.Tracks.Application.UnitOfWork.Repositories;
using EichkustMusic.Tracks.Domain.Entities;
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

        public void Add(Album album)
        {
            _dbContext.Add(album);
        }

        public async Task DeleteAsync(Album album)
        {
            if (album.CoverImagePath != null)
            {
                await _s3.DeleteFileAsync(album.CoverImagePath);
            }

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
    }
}

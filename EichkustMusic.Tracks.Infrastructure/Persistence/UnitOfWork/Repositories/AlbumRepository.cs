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

        public AlbumRepository(TracksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Album album)
        {
            _dbContext.Add(album);
        }

        public void Delete(Album album)
        {
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

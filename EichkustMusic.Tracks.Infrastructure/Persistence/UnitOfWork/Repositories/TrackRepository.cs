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
    public class TrackRepository : ITrackRepository
    {
        private readonly TracksDbContext _dbContext;

        public TrackRepository(TracksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Track track)
        {
            _dbContext.Add(track);
        }

        public void Delete(Track track)
        {
            _dbContext.Remove(track);
        }

        public async Task<Track?> GetByIdAsync(int id)
        {
            return await _dbContext.Tracks
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Track>> ListAsync(int pageNum, int pageSize, string? search)
        {
            IQueryable<Track> tracks = _dbContext.Tracks;

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

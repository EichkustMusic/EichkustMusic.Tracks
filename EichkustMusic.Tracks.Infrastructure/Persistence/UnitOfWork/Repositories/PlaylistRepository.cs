using EichkustMusic.Tracks.Application.UnitOfWork.Repositories;
using EichkustMusic.Tracks.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EichkustMusic.Tracks.Infrastructure.Persistence.UnitOfWork.Repositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly TracksDbContext _dbContext;

        public PlaylistRepository(TracksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Playlist playlist)
        {
            _dbContext.Add(playlist);
        }

        public void AddTrack(Playlist playlist, Track track)
        {
            var playlistTrack = new PlaylistTrack
            {
                TrackId = track.Id,
                PlaylistId = playlist.Id,
            };

            _dbContext.Add(playlistTrack);
        }

        public void Delete(Playlist playlist)
        {
            _dbContext.Remove(playlist);
        }

        public async Task<Playlist?> GetByIdAsync(int id)
        {
            return await _dbContext.Playlists
                .Include(p => p.PlaylistTracks)
                .ThenInclude(p => p.Track)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Playlist>> ListAsync(
            int pageNum, int pageSize, string? search)
        {
            IQueryable<Playlist> playlists = _dbContext.Playlists
                .Include(p => p.PlaylistTracks)
                .ThenInclude(p => p.Track);
            
            if (search != null)
            {
                playlists = playlists
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

            return await playlists
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}

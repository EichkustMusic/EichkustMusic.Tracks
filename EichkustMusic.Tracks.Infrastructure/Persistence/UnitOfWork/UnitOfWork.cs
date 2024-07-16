using EichkustMusic.Tracks.Application.UnitOfWork;
using EichkustMusic.Tracks.Application.UnitOfWork.Repositories;
using EichkustMusic.Tracks.Infrastructure.Persistence.UnitOfWork.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EichkustMusic.Tracks.Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TracksDbContext _dbContext;

        public ITrackRepository TrackRepository { get; }

        public IAlbumRepository AlbumRepository { get; }

        public IPlaylistRepository PlaylistRepository { get; }

        public UnitOfWork(TracksDbContext dbContext)
        {
            _dbContext = dbContext;

            TrackRepository = new TrackRepository(dbContext);
            AlbumRepository = new AlbumRepository(dbContext);
            PlaylistRepository = new PlaylistRepository(dbContext);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}

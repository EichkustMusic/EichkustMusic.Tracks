using EichkustMusic.Tracks.Application.UnitOfWork.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EichkustMusic.Tracks.Application.UnitOfWork
{
    public interface IUnitOfWork
    {
        ITrackRepository TrackRepository { get; }

        IAlbumRepository AlbumRepository { get; }

        IPlaylistRepository PlaylistRepository { get; }

        Task SaveChangesAsync();
    }
}

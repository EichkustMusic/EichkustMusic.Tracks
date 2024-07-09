using EichkustMusic.Tracks.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EichkustMusic.Tracks.Application.UnitOfWork.Repositories
{
    public interface IPlaylistRepository
    {
        Task<Playlist> GetByIdAsync(int id);

        Task<IEnumerable<Playlist>> ListAsync(
            int pageNum, int pageSize, string? search);

        void Delete(Playlist playlist);

        void Add(Playlist playlist);

        void AddTrack(Playlist playlist, Track track);
    }
}

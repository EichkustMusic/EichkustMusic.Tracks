using EichkustMusic.Tracks.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EichkustMusic.Tracks.Application.UnitOfWork.Repositories
{
    public interface ITrackRepository
    {
        Task<Track?> GetByIdAsync(int id);

        Task<IEnumerable<Track>> ListAsync(
            int pageNum, int pageSize, string? search);

        void Delete(Track track);

        void Add(Track track);
    }
}

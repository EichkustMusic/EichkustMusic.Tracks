using EichkustMusic.Tracks.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EichkustMusic.Tracks.Application.UnitOfWork.Repositories
{
    public interface IAlbumRepository
    {
        public Task<Album> GetByIdAsync(int id);

        public Task<IEnumerable<Album>> ListAsync(
            int pageNum, int pageSize, int? search);

        public void Delete(Album album);

        public void Add(Album album);
    }
}

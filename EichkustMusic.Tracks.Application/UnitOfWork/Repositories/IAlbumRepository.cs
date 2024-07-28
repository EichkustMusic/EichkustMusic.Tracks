using EichkustMusic.Tracks.Domain.Entities;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EichkustMusic.Tracks.Application.UnitOfWork.Repositories
{
    public interface IAlbumRepository
    {
        public Task<Album?> GetByIdAsync(int id);

        public Task<IEnumerable<Album>> ListAsync(
            int pageNum, int pageSize, string? search);

        public Task DeleteAsync(Album album);

        public Task ApplyPatchDocumentAsyncTo(Album album, JsonPatchDocument patchDocument);

        public Task AddAsync(Album album);
    }
}

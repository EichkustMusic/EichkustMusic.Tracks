using Asp.Versioning;
using EichkustMusic.Tracks.Application.DTOs.Album;
using EichkustMusic.Tracks.Application.S3;
using EichkustMusic.Tracks.Application.UnitOfWork;
using EichkustMusic.Tracks.Infrastructure.S3;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace EichkustMusic.Tracks.API.Controllers
{
    #region ApiV1
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController : ControllerBase
    {
        public IUnitOfWork _unitOfWork;
        public IS3Storage _s3Storage;

        public AlbumsController(
            IUnitOfWork unitOfWork, IS3Storage s3Storage)
        {
            _unitOfWork = unitOfWork;
            _s3Storage = s3Storage;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AlbumDTO>> GetById(int id)
        {
            var album = await _unitOfWork.AlbumRepository
                .GetByIdAsync(id);

            if (album == null)
            {
                return NotFound(nameof(id));
            }

            var albumDTO = AlbumDTO.MapFromAlbum(album);

            return Ok(albumDTO);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlbumDTO>>> List(
            string? search, int pageNum = 1, int pageSize = 2)
        {
            var albums = await _unitOfWork.AlbumRepository
                .ListAsync(pageNum, pageSize, search);

            var albumDTOs = new List<AlbumDTO>();

            foreach (var album in albums)
            {
                albumDTOs.Add(AlbumDTO.MapFromAlbum(album));
            }

            return Ok(albumDTOs);
        }

        [HttpPost]
        public async Task<ActionResult<AlbumCreateResultDTO>> Create(
            AlbumForCreateDTO albumForCreateDTO)
        {
            var album = albumForCreateDTO.MapToAlbum();

            foreach (var trackId in albumForCreateDTO.TracksIds)
            {
                var track = await _unitOfWork.TrackRepository
                    .GetByIdAsync(trackId);

                if (track == null)
                {
                    return NotFound(nameof(track));
                }

                album.Tracks.Add(track);
            }

            _unitOfWork.AlbumRepository.Add(album);

            await _unitOfWork.SaveChangesAsync();

            var albumDTO = AlbumCreateResultDTO.MapFromAlbum(album);

            albumDTO.PathToUploadCoverImage = _s3Storage.GetPreSignedUploadUrl(BucketNames.AlbumCovers);

            return CreatedAtAction(nameof(GetById), new
            {
                album.Id,
            },
            albumDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var album = await _unitOfWork.AlbumRepository
                .GetByIdAsync(id);

            if (album == null)
            {
                return NotFound(nameof(id));
            }

            await _unitOfWork.AlbumRepository.DeleteAsync(album);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(
            int id, JsonPatchDocument patchDocument)
        {
            var album = await _unitOfWork.AlbumRepository
                .GetByIdAsync(id);

            if (album == null)
            {
                return NotFound(nameof(id));
            }

            patchDocument.ApplyTo(album);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
    #endregion
}

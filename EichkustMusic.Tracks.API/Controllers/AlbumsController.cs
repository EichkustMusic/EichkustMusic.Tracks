﻿using Asp.Versioning;
using EichkustMusic.S3;
using EichkustMusic.Tracks.Application.DTOs.Album;
using EichkustMusic.Tracks.Application.UnitOfWork;
using EichkustMusic.Tracks.Application.UnitOfWork.Exceptions;
using EichkustMusic.Tracks.Infrastructure.S3;
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
        public IS3Storage _s3;

        public AlbumsController(
            IUnitOfWork unitOfWork, IS3Storage s3)
        {
            _unitOfWork = unitOfWork;
            _s3 = s3;
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

            await _unitOfWork.AlbumRepository.AddAsync(album);

            await _unitOfWork.SaveChangesAsync();

            var albumDTO = AlbumCreateResultDTO.MapFromAlbum(album);

            albumDTO.PathToUploadCoverImage = _s3.GetPreSignedUploadUrl(BucketNames.AlbumCovers);

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

            try
            {
                await _unitOfWork.AlbumRepository
                    .ApplyPatchDocumentAsyncTo(album, patchDocument);

                await _unitOfWork.SaveChangesAsync();

                return NoContent();
            }
            catch (NewFileNotFound exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("get_presigned_upload_url_for/{bucketName}")]
        public ActionResult<string> GetPresignedUploadURL(
            string bucketName)
        {
            string bucketNameForS3;

            if (bucketName == "cover_image")
            {
                bucketNameForS3 = BucketNames.AlbumCovers;
            }
            else
            {
                return BadRequest(nameof(bucketName));
            }


            return Ok(_s3.GetPreSignedUploadUrl(bucketNameForS3));
        }
    }
    #endregion
}

using Asp.Versioning;
using EichkustMusic.Tracks.Application.DTOs.Track;
using EichkustMusic.Tracks.Application.S3;
using EichkustMusic.Tracks.Infrastructure.S3;
using EichkustMusic.Tracks.Application.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using EichkustMusic.Tracks.Application.UnitOfWork.Exceptions;

namespace EichkustMusic.Tracks.API.Controllers
{
    #region ApiV1
    [ApiVersion(1.0)]
    [Route("api/[controller]")]
    [ApiController]
    public class TracksController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IS3Storage _s3;

        public TracksController(IUnitOfWork unitOfWork, IS3Storage s3)
        {
            _unitOfWork = unitOfWork;
            _s3 = s3;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrackDTO>>> List(
             string? search, int pageNum = 1, int pageSize = 5)
        {
            var tracks = await _unitOfWork.TrackRepository
                .ListAsync(pageNum, pageSize, search);

            var trackDTOs = new List<TrackDTO>();

            foreach (var track in tracks)
            {
                var trackDTO = TrackDTO.MapFromTrack(track, withAlbum: true);

                trackDTOs.Add(trackDTO);
            }

            return Ok(trackDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TrackDTO>> GetById(int id)
        {
            var track = await _unitOfWork.TrackRepository
                .GetByIdAsync(id);

            if (track == null)
            {
                return NotFound(nameof(id));
            }

            var trackDTO = TrackDTO.MapFromTrack(track);

            return Ok(trackDTO);
        }

        [HttpPost]
        public async Task<ActionResult<TrackCreateResultDTO>> Create(
            TrackForCreateDTO trackForCreateDTO)
        {
            var track = trackForCreateDTO.MapToTrack();

            _unitOfWork.TrackRepository
                .Add(track);

            await _unitOfWork.SaveChangesAsync();

            var trackCreateResultDTO = TrackCreateResultDTO
                .MapFromTrack(track);

            trackCreateResultDTO.PathToUploadCoverImage = _s3
                .GetPreSignedUploadUrl(BucketNames.TrackCovers);

            trackCreateResultDTO.PathToUploadMusic = _s3
                .GetPreSignedUploadUrl(BucketNames.MusicFiles);

            return CreatedAtAction(nameof(GetById), new
            {
                track.Id
            },
            trackCreateResultDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var track = await _unitOfWork.TrackRepository
                .GetByIdAsync(id);

            if (track == null)
            {
                return NotFound(nameof(id));
            }

            await _unitOfWork.TrackRepository.DeleteAsync(track);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(
            int id, JsonPatchDocument patchDocument)
        {
            var track = await _unitOfWork.TrackRepository
                .GetByIdAsync(id);

            if (track == null)
            {
                return NotFound(nameof(id));
            }

            try
            {
                await _unitOfWork.TrackRepository
                                .ApplyPatchDocumentAsyncTo(track, patchDocument);

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
                bucketNameForS3 = BucketNames.TrackCovers;
            }
            else if (bucketName == "music_file")
            {
                bucketNameForS3 = BucketNames.MusicFiles;
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

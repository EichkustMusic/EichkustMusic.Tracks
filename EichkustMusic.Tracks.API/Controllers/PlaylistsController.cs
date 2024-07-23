using Asp.Versioning;
using EichkustMusic.Tracks.Application.DTOs.Playlist;
using EichkustMusic.Tracks.Application.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace EichkustMusic.Tracks.API.Controllers
{
    [ApiVersion(1.0)]
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlaylistsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlaylistDTO>>> List(
            string? search, int pageNum = 1, int pageSize = 5)
        {
            var playlists = await _unitOfWork.PlaylistRepository
                .ListAsync(pageNum, pageSize, search);

            var playlistDTOs = new List<PlaylistDTO>();

            foreach (var playlist in playlists)
            {
                playlistDTOs.Add(PlaylistDTO.MapFromPlaylist(playlist));
            }

            return Ok(playlistDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlaylistDTO>> GetById(int id)
        {
            var playlist = await _unitOfWork.PlaylistRepository
                .GetByIdAsync(id);

            if (playlist == null)
            {
                return NotFound(nameof(id));
            }

            var playlistDTO = PlaylistDTO.MapFromPlaylist(playlist);

            return Ok(playlistDTO);
        }

        [HttpPost]
        public async Task<ActionResult<PlaylistDTO>> Create(
            PlaylistForCreateDTO playlistForCreateDTO)
        {
            var playlist = playlistForCreateDTO.MapToPlaylist();

            _unitOfWork.PlaylistRepository.Add(playlist);

            await _unitOfWork.SaveChangesAsync();

            foreach (var trackId in playlistForCreateDTO.TracksIds)
            {
                var track = await _unitOfWork.TrackRepository
                    .GetByIdAsync(trackId);

                if (track == null)
                {
                    return NotFound(nameof(trackId));
                }

                _unitOfWork.PlaylistRepository
                    .AddTrack(playlist, track);
            }

            await _unitOfWork.SaveChangesAsync();

            var playlistDTO = PlaylistDTO.MapFromPlaylist(playlist);

            return CreatedAtAction(nameof(GetById), new
                {
                    playlist.Id,
                },
                playlistDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var playlist = await _unitOfWork.PlaylistRepository
                .GetByIdAsync(id);

            if (playlist == null)
            {
                return NotFound(nameof(id));
            }

            _unitOfWork.PlaylistRepository.Delete(playlist);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(
            int id, JsonPatchDocument patchDocument)
        {
            var playlist = await _unitOfWork.PlaylistRepository
                .GetByIdAsync(id);

            if (playlist == null)
            {
                return NotFound(nameof(id));
            }

            patchDocument.ApplyTo(playlist);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{playlistId}/add_track/{trackId}")]
        public async Task<ActionResult> AddTrack(int playlistId, int trackId)
        {
            var playlist = await _unitOfWork.PlaylistRepository
                .GetByIdAsync(playlistId);

            if (playlist == null)
            {
                return NotFound(nameof(playlistId));
            }

            var track = await _unitOfWork.TrackRepository
                .GetByIdAsync(trackId);

            if (track == null)
            {
                return NotFound(nameof(trackId));
            }

            _unitOfWork.PlaylistRepository.AddTrack(playlist, track);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}

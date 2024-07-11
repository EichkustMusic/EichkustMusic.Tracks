using EichkustMusic.Tracks.Application.DTOs.Album;
using EichkustMusic.Tracks.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EichkustMusic.Tracks.Testing.ApplicationTests
{
    public class DTOsTests
    {
        [Test]
        public void AlbumForCreateDTO_MapToAlbum_MapsWithoutExceptions()
        {
            var albumForCreateDto = new AlbumForCreateDTO()
            {
                Name = "name",
                Description = "description",
                TracksIds = [1, 2, 3]
            };

            Assert.DoesNotThrow(() => 
                albumForCreateDto.MapToAlbum());
        } 

        [Test]
        public void AlbumCreateResultDTO_MapFromAlbum_MapsWithoutExceptions()
        {
            var album = new Album()
            {
                Id = 1,
                Name = "name",
                Description = "description",
                Tracks = new List<Track>()
                {
                    new Track()
                    {
                        Id = 1,
                        Description = "description",
                        UserId = 1,
                        CoverImagePath = "path",
                        MusicPath = null,
                        AlbumId = 1,
                        Album = null
                    }
                }
            };

            Assert.DoesNotThrow(() =>
                AlbumCreateResultDTO.MapFromAlbum(album));
        }
    }
}

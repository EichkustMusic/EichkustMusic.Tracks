using EichkustMusic.Tracks.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EichkustMusic.Tracks.Infrastructure.Persistence
{
    public class TracksDbContext : DbContext
    {
        public TracksDbContext(DbContextOptions<TracksDbContext> options) : base(options) {}

        public DbSet<Track> Tracks { get; } = null!;

        public  DbSet<Album> Albums { get; } = null!;

        public DbSet<Playlist> Playlists { get; } = null!;

        public DbSet<PlaylistTrack> PlaylistTrack { get; } = null!;
    }
}

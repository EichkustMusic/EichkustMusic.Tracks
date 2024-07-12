using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EichkustMusic.Tracks.Infrastructure.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(
            this IServiceCollection services, IConfigurationManager configurationManager)
        {
            return services.AddDbContext<TracksDbContext>(o =>
                o.UseNpgsql(configurationManager.GetConnectionString("TracksDb")));
        }
    }
}

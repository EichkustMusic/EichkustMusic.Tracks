using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EichkustMusic.Tracks.Infrastructure.S3
{
    internal class S3Config
    {
        public string AccessKey { get; set; } = null!;
        public string SecretKey { get; set; } = null!;
    }
}

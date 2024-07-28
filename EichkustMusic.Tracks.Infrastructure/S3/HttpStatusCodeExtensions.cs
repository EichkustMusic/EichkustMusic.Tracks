using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EichkustMusic.Tracks.Infrastructure.S3
{
    public static class HttpStatusCodeExtensions
    {
        public static bool IsSuccess(this HttpStatusCode httpStatusCode)
            => (int)httpStatusCode >= 200 && (int)httpStatusCode <= 299;
    }
}

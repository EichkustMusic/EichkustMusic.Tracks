using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EichkustMusic.Tracks.Application.S3
{
    public interface IS3Storage
    {
        public string GetPreSignedUploadUrl(string bucketName);
    }
}

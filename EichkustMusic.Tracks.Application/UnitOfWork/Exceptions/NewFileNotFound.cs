using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EichkustMusic.Tracks.Application.UnitOfWork.Exceptions
{
    public class NewFileNotFound(string fileURL) : Exception(
        $"File at {fileURL} doesn't exist at the S3. You should upload it before change an entity.")
    { }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EichkustMusic.Tracks.Application.S3.Exceptions
{
    public class S3FileDeleteException(string message) : Exception(message)
    { }
}

using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Amazon.S3.Model;
using EichkustMusic.Tracks.Application.S3;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EichkustMusic.Tracks.Infrastructure.S3
{
    public class S3Storage : IS3Storage
    {
        private readonly AmazonS3Client _s3;

        public S3Storage(IConfigurationManager configuration)
        {
            var accessKey = configuration["S3:AccessKey"];
            var secretKey = configuration["S3:SecretKey"];

            if (accessKey == null || secretKey == null)
            {
                throw new Exception("Secret key or access key for S3 is null");
            }

            var s3Config = new AmazonS3Config()
            {
                ServiceURL = "https://s3.wasabisys.com",
            };

            _s3 = new AmazonS3Client(accessKey, secretKey, s3Config);
        }

        public async Task<bool> DeleteFileAsync(string fileURL)
        {
            var splitedUrl = fileURL.Split("/");

            var fileName = splitedUrl[^1];
            var bucketName = splitedUrl[^2];

            var deleteFileRequst = new DeleteObjectRequest()
            {
                BucketName = bucketName,
                Key = fileName,
            };

            var response = await _s3.DeleteObjectAsync(deleteFileRequst);

            return response.HttpStatusCode.IsSuccess();
        }

        public string GetPreSignedUploadUrl(string bucketName)
        {
            var objectName = Guid.NewGuid().ToString();

            var getPreSignedUrlRequest = new GetPreSignedUrlRequest()
            {
                BucketName = bucketName,
                Key = objectName,
                Expires = DateTime.UtcNow.AddMinutes(1),
            };

            return _s3.GetPreSignedURL(getPreSignedUrlRequest);
        }
    }
}

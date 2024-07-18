using Amazon.S3.Model;
using dotenv.net;
using EichkustMusic.Tracks.Application.S3;
using EichkustMusic.Tracks.Infrastructure.S3;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EichkustMusic.Tracks.Testing.InfrastructureTests.S3Tests
{
    public class S3StorageTests
    {
        private readonly IS3Storage _s3Storage;

        public S3StorageTests()
        {
            // .env file should be created at bin/Env/net8.0/
            var directory = $"{Directory.GetCurrentDirectory()}" + "/.env";

            Debug.WriteLine(directory);

            DotNetEnv.Env.Load(directory);

            var configurationManagerMock = new Mock<IConfigurationManager>();

            var accessKey = Environment.GetEnvironmentVariable("S3_ACCESS_KEY");
            var secretKey = Environment.GetEnvironmentVariable("S3_SECRET_KEY");

            configurationManagerMock
                .Setup(cm => cm["S3:AccessKey"])
                .Returns(accessKey);

            configurationManagerMock
                .Setup(cm => cm["S3:SecretKey"])
                .Returns(secretKey);

            _s3Storage = new S3Storage(configurationManagerMock.Object);
        }

        [Test]
        public void S3Storage_GetPreSignedUploadUrl_ReturnsUrlIfCorrectBucketNameAndCredentials()
        {
            var actual = _s3Storage.GetPreSignedUploadUrl(BucketNames.TrackCovers);

            Console.WriteLine(actual);

            Assert.That(actual, Is.Not.Null);
        }
    }
}

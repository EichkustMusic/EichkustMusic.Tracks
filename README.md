# EichkustMusic.Tracks
**ASP.Net Core tracks microservice.** Works with tracks, albums and playlists.

## API versions
- 1.0 - actual

## Configure and run application
1. Create *appsettings.json* in *./EichkustMusic.Tracks.API*
```
{
    "ConnectionStrings": {
      "TracksDb": (postgresql connection string)
    },
    // Wasabi S3
    "S3": {
      "AccessKey": ...,
      "SecretKey": ...
    }
  }
```
2. ```dotnet run --project EichkustMusic.Tracks.API```

## Configure and run tests
1. Create *.env* in *./EichkustMusic.Tracks.Testing/bin/Env/net8.0/*, where *Env* is test environment (For example, *Debug*)
```
S3_ACCESS_KEY= ...
S3_SECRET_KEY= ...
```
2. The easiest way to run test is to use *Test Explorer* in *Visual Studio* (https://learn.microsoft.com/en-us/visualstudio/test/using-code-coverage-to-determine-how-much-code-is-being-tested)
- **You should run each test separately!**
- **You should rollback S3 after each test manually in _v1_ release!**

## All endpoints are mapped to the Postman collection
https://www.postman.com/winter-trinity-283337/workspace/eichkustmusic/collection/25734689-fa42e932-c922-462a-bf6b-6794e2dcdadf?action=share&creator=25734689
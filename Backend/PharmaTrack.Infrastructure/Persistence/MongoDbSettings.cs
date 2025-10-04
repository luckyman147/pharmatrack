using System;

namespace PharmaTrack.Infrastructure.Persistence;

public class MongoDbSettings
{
  public string MongoDbConnection { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
}

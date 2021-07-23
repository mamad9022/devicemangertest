using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.Extensions.Options;
using Sepid.DeviceManagerTest.Common.Options;
using StackExchange.Redis;

namespace Sepid.DeviceManagerTest.Common.Redis
{
    public class RedisService : IRedisServices
    {
        private readonly IOptions<RedisConfiguration> _redisConfiguration;

        public RedisService(IOptions<RedisConfiguration> redisConfiguration)
        {
            _redisConfiguration = redisConfiguration;
        }

        public List<RedisKey> GetAllRedisKey()
        {
            ConfigurationOptions options = ConfigurationOptions.Parse(_redisConfiguration.Value.Connection);
            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(options);

            EndPoint endPoint = connection.GetEndPoints().First();

            return connection.GetServer(endPoint).Keys(pattern: "*").ToList();
        }

        public List<RedisKey> GetAllRedisKeyContain(string code)
        {
            ConfigurationOptions options = ConfigurationOptions.Parse(_redisConfiguration.Value.Connection);
            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(options);

            EndPoint endPoint = connection.GetEndPoints().First();

            return connection.GetServer(endPoint).Keys(pattern: code).ToList();
        }


    }
}
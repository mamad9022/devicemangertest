using System.Collections.Generic;
using StackExchange.Redis;

namespace Sepid.DeviceManagerTest.Common.Redis
{
    public interface IRedisServices
    {
        List<RedisKey> GetAllRedisKey();

        List<RedisKey> GetAllRedisKeyContain(string code);
    }
}
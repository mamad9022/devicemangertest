using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Sepid.DeviceManagerTest.Common.Helper
{
    public static class DistributedCacheExtensions
    {
        public static async Task SetRecordAsync<T>(this IDistributedCache cache, string key, T record, CancellationToken cancellationToken,
            TimeSpan? absoluteExpireTime = null, TimeSpan? unUsedExpiredTime = null)
        {
            var option = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60),
                SlidingExpiration = unUsedExpiredTime
            };

            var data = JsonConvert.SerializeObject(record,Formatting.Indented,new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            await cache.SetStringAsync(key, data, option, cancellationToken);
        }


        public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string key, CancellationToken cancellationToken)
        {
            var data = await cache.GetStringAsync(key, cancellationToken);

            if (data is null)
                return default;

            return JsonConvert.DeserializeObject<T>(data);
        }

        public static T GetRecord<T>(this IDistributedCache cache, string key)
        {
            var data =  cache.GetString(key);

            if (data is null)
                return default;

            return JsonSerializer.Deserialize<T>(data);
        }

        public static async Task RemoveRecordAsync(this IDistributedCache cache, string key,CancellationToken cancellationToken)
        {
            await cache.RemoveAsync(key, cancellationToken);
        }
    }
}
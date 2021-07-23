using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sepid.DeviceManagerTest.Common.RabbitMq
{
    public interface IBusSubscribe
    {
        void Subscribe(string queueName, Func<string, IDictionary<string, object>, Task<bool>> callBack);
    }
}
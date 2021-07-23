using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sepid.DeviceManagerTest.Client.RabbitMq
{
    public interface IDeviceSubscriber
    {
        void SubscribeLog(Func<string, IDictionary<string, object>, Task<bool>> callBack);

        void SubscribeUpdateDevice(Func<string, IDictionary<string, object>, Task<bool>> callBack);


    }
}
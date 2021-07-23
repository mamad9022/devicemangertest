using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sepid.DeviceManagerTest.Application.Common.Interfaces
{
    public interface IBatchOperationService
    {
        Task SetTime(long groupId,DateTime dateTime,CancellationToken cancellationToken);

        Task LockDevice(long groupId,CancellationToken cancellationToken);

        Task UnlockDevice(long groupId, CancellationToken cancellationToken);

        Task ClearLog(long groupIds, CancellationToken cancellationToken);
    }
}
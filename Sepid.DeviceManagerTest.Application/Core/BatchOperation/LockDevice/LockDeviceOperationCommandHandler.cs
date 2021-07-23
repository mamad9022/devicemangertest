using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using MediatR;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.BatchOperation.LockDevice
{
    public class LockDeviceOperationCommandHandler : IRequestHandler<LockDeviceOperationCommand, Result>
    {
        private readonly IBatchOperationService _batchOperationService;

        public LockDeviceOperationCommandHandler(IBatchOperationService batchOperationService)
        {
            _batchOperationService = batchOperationService;
        }

        public Task<Result> Handle(LockDeviceOperationCommand request, CancellationToken cancellationToken)
        {
            foreach (var groupId in request.GroupIds)
            {
                BackgroundJob.Enqueue(() => _batchOperationService.LockDevice(groupId,cancellationToken));
            }

            return  Task.FromResult(Result.SuccessFul());
        }
    }
}
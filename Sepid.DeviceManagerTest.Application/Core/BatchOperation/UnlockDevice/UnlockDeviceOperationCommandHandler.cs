using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using MediatR;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.BatchOperation.UnlockDevice
{
    public class UnlockDeviceOperationCommandHandler : IRequestHandler<UnlockDeviceOperationCommand, Result>
    {
        private readonly IBatchOperationService _batchOperationService;

        public UnlockDeviceOperationCommandHandler(IBatchOperationService batchOperationService)
        {
            _batchOperationService = batchOperationService;
        }

        public Task<Result> Handle(UnlockDeviceOperationCommand request, CancellationToken cancellationToken)
        {
            foreach (var groupId in request.GroupIds)
            {
                BackgroundJob.Enqueue(() => _batchOperationService.UnlockDevice(groupId, cancellationToken));

            }

            return Task.FromResult(Result.SuccessFul());
        }
    }
}
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using MediatR;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.BatchOperation.SetTime
{
    public class SetTimeOperationCommandHandler : IRequestHandler<SetTimeOperationCommand, Result>
    {
        private readonly IBatchOperationService _batchOperationService;

        public SetTimeOperationCommandHandler(IBatchOperationService batchOperationService)
        {
            _batchOperationService = batchOperationService;
        }

        public Task<Result> Handle(SetTimeOperationCommand request, CancellationToken cancellationToken)
        {
            foreach (var groupId in request.GroupIds)
            {
                BackgroundJob.Enqueue(() => _batchOperationService.SetTime(groupId, request.Date, cancellationToken));
            }

            return Task.FromResult(Result.SuccessFul());
        }
    }
}
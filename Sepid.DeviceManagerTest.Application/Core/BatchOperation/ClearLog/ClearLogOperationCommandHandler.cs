using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using MediatR;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.BatchOperation.ClearLog
{
    public class ClearLogOperationCommandHandler : IRequestHandler<ClearLogOperationCommand, Result>
    {
        private readonly IBatchOperationService _batchOperationService;

        public ClearLogOperationCommandHandler(IBatchOperationService batchOperationService)
        {
            _batchOperationService = batchOperationService;
        }

        public Task<Result> Handle(ClearLogOperationCommand request, CancellationToken cancellationToken)
        {
            foreach (var groupId in request.GroupIds)
            {

                BackgroundJob.Enqueue(() => _batchOperationService.ClearLog(groupId, cancellationToken));

            }

            return Task.FromResult(Result.SuccessFul());
        }
    }
}
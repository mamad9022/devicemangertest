using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using MediatR;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.Settings.Command.SyncDeviceDatabase
{
    public class SyncDeviceDatabaseCommandHandler : IRequestHandler<SyncDeviceDatabaseCommand, Result>
    {
        private readonly IDeviceServices _deviceServices;

        public SyncDeviceDatabaseCommandHandler(IDeviceServices deviceServices)
        {
            _deviceServices = deviceServices;
        }

        public Task<Result> Handle(SyncDeviceDatabaseCommand request, CancellationToken cancellationToken)
        {
            BackgroundJob.Enqueue(() => _deviceServices.SyncDeviceDatabase());

            return Task.FromResult(Result.SuccessFul());
        }
    }
}
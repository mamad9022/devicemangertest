using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.RabbitMq;
using Sepid.DeviceManagerTest.Common.Results;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Sepid.DeviceManagerTest.Common.Localization;

namespace Sepid.DeviceManagerTest.Application.Core.Devices.Command.Delete
{
    public class DeleteDeviceCommandHandler : IRequestHandler<DeleteDeviceCommand, Result>
    {
        private readonly IDeviceManagerContext _context;
        private readonly IBusPublish _busPublish;
        private readonly ILocalization _localization;

        public DeleteDeviceCommandHandler(IDeviceManagerContext context, IBusPublish busPublish, ILocalization localization)
        {
            _context = context;
            _busPublish = busPublish;
            _localization = localization;
        }

        public async Task<Result> Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
        {
            var device = await _context.Devices.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (device is null)
                return Result.Failed(new NotFoundObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.DeviceNotFound,cancellationToken))));


            _busPublish.Send("DeleteDevice", JsonSerializer.Serialize(device));

            _context.Devices.Remove(device);

            await _context.SaveAsync(cancellationToken);

            return Result.SuccessFul();
        }
    }
}
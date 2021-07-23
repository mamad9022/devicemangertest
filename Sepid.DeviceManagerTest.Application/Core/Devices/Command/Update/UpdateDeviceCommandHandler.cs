using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Core.Devices.Dto;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.RabbitMq;
using Sepid.DeviceManagerTest.Common.Results;
using System.Threading;
using System.Threading.Tasks;
using Sepid.DeviceManagerTest.Common.Localization;

namespace Sepid.DeviceManagerTest.Application.Core.Devices.Command.Update
{
    public class UpdateDeviceCommandHandler : IRequestHandler<UpdateDeviceCommand, Result>
    {
        private readonly IMapper _mapper;
        private readonly IDeviceManagerContext _context;
        private readonly IBusPublish _busPublish;
        private readonly ILocalization _localization;

        public UpdateDeviceCommandHandler(IMapper mapper, IDeviceManagerContext context, IBusPublish busPublish, ILocalization localization)
        {
            _mapper = mapper;
            _context = context;
            _busPublish = busPublish;
            _localization = localization;
        }

        public async Task<Result> Handle(UpdateDeviceCommand request, CancellationToken cancellationToken)
        {
            #region Validation

            var device = await _context.Devices.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (device is null)
                return Result.Failed(new NotFoundObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.DeviceNotFound,cancellationToken))));

            var deviceModel =
                await _context.DeviceModels.SingleOrDefaultAsync(x => x.Id == request.DeviceModelId, cancellationToken);

            if (deviceModel is null)
                return Result.Failed(
                    new BadRequestObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.DeviceModelNotFound,cancellationToken))));

            #endregion Validation

            _mapper.Map(request, device);

            await _context.SaveAsync(cancellationToken);

            _busPublish.Send("device", JsonConvert.SerializeObject(_mapper.Map<DeviceDto>(device)));

            return Result.SuccessFul();
        }
    }
}
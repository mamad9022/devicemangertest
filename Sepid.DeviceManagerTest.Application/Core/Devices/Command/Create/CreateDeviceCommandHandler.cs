using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Core.Devices.Dto;
using Sepid.DeviceManagerTest.Application.Models;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.RabbitMq;
using Sepid.DeviceManagerTest.Common.Results;
using System.Threading;
using System.Threading.Tasks;
using Sepid.DeviceManagerTest.Common.Localization;

namespace Sepid.DeviceManagerTest.Application.Core.Devices.Command.Create
{
    public class CreateDeviceCommandHandler : IRequestHandler<CreateDeviceCommand, Result<DeviceDto>>
    {
        private readonly IMapper _mapper;
        private readonly IDeviceManagerContext _context;
        private readonly IBusPublish _busPublish;
        private readonly ILocalization _localization;

        public CreateDeviceCommandHandler(IMapper mapper, IDeviceManagerContext context, IBusPublish busPublish, ILocalization localization)
        {
            _mapper = mapper;
            _context = context;
            _busPublish = busPublish;
            _localization = localization;
        }

        public async Task<Result<DeviceDto>> Handle(CreateDeviceCommand request, CancellationToken cancellationToken)
        {
            #region Validation

            var device = _mapper.Map<Device>(request);

            var deviceModel =
                await _context.DeviceModels.SingleOrDefaultAsync(x => x.Id == request.DeviceModelId, cancellationToken);

            if (deviceModel is null)
                return Result<DeviceDto>.Failed(
                    new BadRequestObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.DeviceModelNotFound, cancellationToken))));

            #endregion Validation


            await _context.Devices.AddAsync(device, cancellationToken);

            await _context.SaveAsync(cancellationToken);

            _busPublish.Send("device", JsonConvert.SerializeObject(_mapper.Map<DeviceDto>(device)));

            return Result<DeviceDto>.SuccessFul(_mapper.Map<DeviceDto>(device));
        }
    }
}
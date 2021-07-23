using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Common.Strategy;
using Sepid.DeviceManagerTest.Common.Dto;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Results;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Sepid.DeviceManagerTest.Common.Localization;

namespace Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Queries
{
    public class
        GetNetworkConfigDeviceQueryHandler : IRequestHandler<GetNetworkConfigDeviceQuery, Result<NetworkInfoDto>>
    {
        private readonly IDeviceManagerContext _context;
        private readonly IMapper _mapper;
        private readonly ILocalization _localization;

        public GetNetworkConfigDeviceQueryHandler(IDeviceManagerContext context, IMapper mapper,
            ILocalization localization)
        {
            _context = context;
            _mapper = mapper;
            _localization = localization;
        }

        public async Task<Result<NetworkInfoDto>> Handle(GetNetworkConfigDeviceQuery request,
            CancellationToken cancellationToken)
        {
            #region Find Device

            var device = await _context.Devices
                .Include(x => x.DeviceModel)
                .SingleOrDefaultAsync(x => x.Id == request.DeviceId, cancellationToken);

            if (device is null)
                return Result<NetworkInfoDto>.Failed(new NotFoundObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.DeviceNotFound, cancellationToken))));

            if (device.IsConnected == false)
                return Result<NetworkInfoDto>.Failed(new BadRequestObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.ConnectionLost, cancellationToken))));

            #endregion Find Device

            #region Set Strategy

            ServiceStrategyContext context = new ServiceStrategyContext();

            context.DetectServices(device.DeviceModel.SdkType);

            #endregion Set Strategy

            var result = context.GetNetworkInfo(new BaseDeviceInfoDto
            {
                Ip = device.Ip,
                Serial = device.Serial,
                Port = device.Port,
                Code = device.DeviceModel.Code
            });

            if (result.Success == false)
            {

                return Result<NetworkInfoDto>.SuccessFul(_mapper.Map<NetworkInfoDto>(device));
            }

            _mapper.Map(result.Data, device);

            await _context.SaveAsync(cancellationToken);

            return Result<NetworkInfoDto>.SuccessFul(result.Data);
        }
    }
}
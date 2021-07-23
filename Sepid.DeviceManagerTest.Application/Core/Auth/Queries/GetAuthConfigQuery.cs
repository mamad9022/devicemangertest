using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Common.Strategy;
using Sepid.DeviceManagerTest.Common.Dto;
using Sepid.DeviceManagerTest.Common.Dto.AuthConfig;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Localization;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.Auth.Queries
{
    public class GetAuthConfigQuery : IRequest<Result<AuthConfigDto>>
    {
        public int DeviceId { get; set; }
    }

    public class GetAuthConfigQueryHandler : IRequestHandler<GetAuthConfigQuery, Result<AuthConfigDto>>
    {
        private readonly IDeviceManagerContext _context;
        private readonly ILocalization _localization;

        public GetAuthConfigQueryHandler(IDeviceManagerContext context, ILocalization localization)
        {
            _context = context;
            _localization = localization;
        }

        public async Task<Result<AuthConfigDto>> Handle(GetAuthConfigQuery request, CancellationToken cancellationToken)
        {
            #region Find Device

            var device = await _context.Devices
                .Include(x => x.DeviceModel)
                .SingleOrDefaultAsync(x => x.Id == request.DeviceId, cancellationToken);

            if (device is null)
                return Result<AuthConfigDto>.Failed(
                    new NotFoundObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.DeviceNotFound,cancellationToken))));

            if (device.IsConnected == false)
                return Result<AuthConfigDto>.Failed(
                    new BadRequestObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.ConnectionLost,cancellationToken))));

            #endregion Find Device

            #region Set Strategy

            ServiceStrategyContext context = new ServiceStrategyContext();

            context.DetectServices(device.DeviceModel.SdkType);

            #endregion Set Strategy

            var result = context.GetAuthConfig(new BaseDeviceInfoDto
            {
                Serial = device.Serial,
                Ip = device.Ip,
                Port = device.Port,
            });

            if (result.Success == false)
                return Result<AuthConfigDto>.Failed(new BadRequestObjectResult(new ApiMessage(result.Message)));


            return Result<AuthConfigDto>.SuccessFul(result.Data);
        }
    }
}
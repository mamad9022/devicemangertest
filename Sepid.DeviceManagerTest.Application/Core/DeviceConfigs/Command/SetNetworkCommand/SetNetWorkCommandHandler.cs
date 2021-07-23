using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Common.Strategy;
using Sepid.DeviceManagerTest.Common.Dto;
using Sepid.DeviceManagerTest.Common.Enum;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Localization;
using Sepid.DeviceManagerTest.Common.Results;
using System.Threading;
using System.Threading.Tasks;

namespace Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Command.SetNetworkCommand
{
    public class SetNetWorkCommandHandler : IRequestHandler<SetNetWorkCommand, Result<NetworkInfoDto>>
    {
        private readonly IDeviceManagerContext _context;
        private readonly IMapper _mapper;
        private readonly ILocalization _localization;
        //private readonly ITemplateService _templateService;

        public SetNetWorkCommandHandler(IDeviceManagerContext context, IMapper mapper, ILocalization localization)
        {
            _context = context;
            _mapper = mapper;
            _localization = localization;
            //_templateService = templateService;
        }

        public async Task<Result<NetworkInfoDto>> Handle(SetNetWorkCommand request, CancellationToken cancellationToken)
        {
            #region Find Device

            var device = await _context.Devices
                .Include(x => x.DeviceModel)
                .SingleOrDefaultAsync(x => x.Id == request.DeviceId, cancellationToken);

            if (device is null)
                return Result<NetworkInfoDto>.Failed(new NotFoundObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.DeviceNotFound, cancellationToken))));

            if (device.IsConnected == false)
                return Result<NetworkInfoDto>.Failed(new BadRequestObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.ConnectionLost, cancellationToken))));

            #endregion Find Device

            #region Detect Strategy Interface

            ServiceStrategyContext context = new();

            context.DetectServices(device.DeviceModel.SdkType);


            #endregion Detect Strategy Interface

            var result = context.SetNetWorkConfig(new BaseDeviceInfoDto
            {
                Ip = device.Ip,
                Serial = device.Serial,
                Port = device.Port,
                Code = device.DeviceModel.Code,
                ServerIp = device.ServerIp,
                ServerPort = device.ServerPort,

            }, _mapper.Map<NetworkInfoDto>(request));

            if (request.IsMatchOnServer)
            {
                context.DeleteAllUser(new BaseDeviceInfoDto
                {
                    Ip = device.Ip,
                    Serial = device.Serial,
                    Port = device.Port,
                    Code = device.DeviceModel.Code,
                    ServerIp = device.ServerIp,
                    ServerPort = device.ServerPort,

                });

               // context.DetectMatching(SdkType.BioStarV2);

                //context.ServerMatching(new BaseDeviceInfoDto
                //{
                //    Ip = device.Ip,
                //    Serial = device.Serial,
                //    Port = device.Port,
                //    Code = device.DeviceModel.Code,
                //    ServerIp = device.ServerIp,
                //    ServerPort = device.ServerPort
                //});


            }

            if (request.IsMatchOnServer == false)
            {
                //context.DetectMatching(SdkType.BioStarV2);

                //context.DetachServerMatching(new BaseDeviceInfoDto
                //{
                //    Ip = device.Ip,
                //    Serial = device.Serial,
                //    Port = device.Port,
                //    Code = device.DeviceModel.Code,
                //    ServerIp = device.ServerIp,
                //    ServerPort = device.ServerPort
                //});

            }


            if (result.Success == false)
                return Result<NetworkInfoDto>.Failed(
                    new BadRequestObjectResult(new ApiMessage(result.Message)));

            //if device ip change effect in evacuation log so we should update
            //ip and port if changes to request to new ip address

            #region Update Device

            _mapper.Map(result.Data, device);

            device.IsMatchOnServer = request.IsMatchOnServer;


            await _context.SaveAsync(cancellationToken);


            #endregion Update Device

            //context.ConnectionStatus(new BaseDeviceInfoDto
            //{
            //    Ip = result.Data.Ip,
            //    Serial = result.Data.Serial,
            //    Port = (int)result.Data.Port,
            //    ServerIp = result.Data.ServerAddress,
            //    ServerPort = (int)result.Data.ServerPort,
            //    Code = device.DeviceModel.Code
            //});

            result.Data.IsMatchOnServer = request.IsMatchOnServer;

            return Result<NetworkInfoDto>.SuccessFul(result.Data);
        }
    }
}
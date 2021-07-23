using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Common.Strategy;
using Sepid.DeviceManagerTest.Common.Dto;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Results;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NETCore.Encrypt;
using Sepid.DeviceManagerTest.Common.Enum;
using Sepid.DeviceManagerTest.Common.Localization;

namespace Sepid.DeviceManagerTest.Application.Core.User.Queries
{
    public class GetUserListQuery : IRequest<Result<List<UserDto>>>
    {
        public int DeviceId { get; set; }
    }

    public class GetUserListQueryHandler : IRequestHandler<GetUserListQuery, Result<List<UserDto>>>
    {
        private readonly IDeviceManagerContext _context;
        private readonly ILocalization _localization;
        private const string KEY = "92A826412D074EB7B6D4712C4C9237E1";
        private const string IV = "1891823B13D344B6";

        public GetUserListQueryHandler(IDeviceManagerContext context, ILocalization localization)
        {
            _context = context;
            _localization = localization;
        }

        public async Task<Result<List<UserDto>>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            #region Find Device

            var device = await _context.Devices
                .Include(x => x.DeviceModel)
                .SingleOrDefaultAsync(x => x.Id == request.DeviceId, cancellationToken);

            if (device is null)
                return Result<List<UserDto>>.Failed(new NotFoundObjectResult(
                    new ApiMessage(await _localization.GetMessage(ResponseMessage.DeviceNotFound, cancellationToken))));

            #endregion Find Device

            #region Detect Strategy Interface

            ServiceStrategyContext context = new ServiceStrategyContext();

            context.DetectServices(device.DeviceModel.SdkType);

            #endregion Detect Strategy Interface

            var result = context.GetAllUser(new BaseDeviceInfoDto
            {
                Serial = device.Serial,
                Ip = device.Ip,
                Port = device.Port,
                Code = device.DeviceModel.Code
            });

            if (result.Success == false)
                return Result<List<UserDto>>.Failed(new BadRequestObjectResult(new ApiMessage(result.Message)));

            EncryptUserTemplate(result);


            return Result<List<UserDto>>.SuccessFul(result.Data);
        }

        private static void EncryptUserTemplate(Result<List<UserDto>> result)
        {
            result.Data.ForEach(user =>
                user.Templates.ForEach(template =>
                {
                    if (template.TemplateType == TemplateType.Finger || template.TemplateType == TemplateType.Face)
                    {
                        template.TemplateData = EncryptProvider.AESEncrypt(template.TemplateData, KEY, IV);
                    }
                }));
        }
    }
}
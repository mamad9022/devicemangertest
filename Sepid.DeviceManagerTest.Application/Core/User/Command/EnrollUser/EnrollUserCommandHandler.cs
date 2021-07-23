using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NETCore.Encrypt;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Common.Strategy;
using Sepid.DeviceManagerTest.Application.Models;
using Sepid.DeviceManagerTest.Common.Dto;
using Sepid.DeviceManagerTest.Common.Enum;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Localization;
using Sepid.DeviceManagerTest.Common.Results;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Sepid.DeviceManagerTest.Application.Core.User.Command.EnrollUser
{
    public class EnrollUserCommandHandler : IRequestHandler<EnrollUserCommand, Result>
    {
        private readonly IDeviceManagerContext _context;
        private readonly IMapper _mapper;
        private readonly ILocalization _localization;
        private const string KEY = "92A826412D074EB7B6D4712C4C9237E1";
        private const string IV = "1891823B13D344B6";

        public EnrollUserCommandHandler(IDeviceManagerContext context, IMapper mapper, ILocalization localization)
        {
            _context = context;
            _mapper = mapper;
            _localization = localization;
        }

        public async Task<Result> Handle(EnrollUserCommand request, CancellationToken cancellationToken)
        {
            #region Find Device

            var device = await _context.Devices
                .Include(x => x.DeviceModel)
                .SingleOrDefaultAsync(x => x.Serial == request.DeviceSerial, cancellationToken);

            if (device is null)
                return Result.Failed(new NotFoundObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.DeviceNotFound, cancellationToken))));


            #endregion Find Device

            #region Detect Strategy Interface

            ServiceStrategyContext context = new ServiceStrategyContext();

            context.DetectServices(device.DeviceModel.SdkType);

            #endregion Detect Strategy Interface

            var userDto = _mapper.Map<UserDto>(request);

            DecryptTemplate(userDto);


            var result = context.EnrollUser(new BaseDeviceInfoDto
            {
                Code = device.DeviceModel.Code,
                Ip = device.Ip,
                Port = device.Port,
                Serial = device.Serial
            }, userDto);

            await _context.TransferLogs.AddAsync(new TransferLog
            {
                IsSuccess = result.Success,
                CreateDate = DateTime.Now,
                TransferLogType = TransferLogType.Enrollment,
                Retry = 1,
                Description = $"{request.Name} - {request.Code}",
                ErrorMessage = result.Success == false ? result.Message : "",
                Data = JsonSerializer.Serialize(request),
                DeviceId = device.Id,
                Reason = result.Reason

            }, cancellationToken);

            await _context.SaveAsync(cancellationToken);

            if (result.Success == false)
                return Result.Failed(new BadRequestObjectResult(new ApiMessage(result.Message)));



            return Result.SuccessFul();
        }

        private static void DecryptTemplate(UserDto userDto)
        {
            userDto.Templates.ForEach(template =>
            {
                if (template.TemplateType == TemplateType.Finger || template.TemplateType == TemplateType.Face)
                {
                    template.TemplateData = EncryptProvider.AESDecrypt(template.TemplateData, KEY, IV);
                }
            });
        }
    }
}
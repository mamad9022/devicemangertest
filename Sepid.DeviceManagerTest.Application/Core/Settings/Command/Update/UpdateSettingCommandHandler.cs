using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NETCore.Encrypt;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Localization;
using Sepid.DeviceManagerTest.Common.RabbitMq;
using Sepid.DeviceManagerTest.Common.Results;
using Serilog;

namespace Sepid.DeviceManagerTest.Application.Core.Settings.Command.Update
{
    public class UpdateSettingCommandHandler : IRequestHandler<UpdateSettingCommand, Result>
    {
        private readonly IDeviceManagerContext _context;
        private readonly IMapper _mapper;
        private readonly IBusPublish _busPublish;
        private readonly IMemoryCache _cache;
        private readonly ILocalization _localization;
        private const string KEY = "BDE6282501A84E0DB9E777D0AE37FED4";
        private const string IV = "2A5658A3A7204E19";

        public UpdateSettingCommandHandler(IDeviceManagerContext context, IMapper mapper, IBusPublish busPublish, IMemoryCache cache, ILocalization localization)
        {
            _context = context;
            _mapper = mapper;
            _busPublish = busPublish;
            _cache = cache;
            _localization = localization;
        }

        public async Task<Result> Handle(UpdateSettingCommand request, CancellationToken cancellationToken)
        {
            var setting = await _context.Settings.FirstOrDefaultAsync(cancellationToken);

            _mapper.Map(request, setting);

            await _context.SaveAsync(cancellationToken);

            if (!string.IsNullOrWhiteSpace(request.License))
            {
                try
                {
                    var license = EncryptProvider.AESDecrypt(request.License, KEY, IV);

                    if (string.IsNullOrWhiteSpace(license))
                    {
                        return Result.Failed(new BadRequestObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.InvalidLicense, cancellationToken))));
                    }

                }
                catch (Exception e)
                {
                    Log.Error(e.Message, e.StackTrace);

                    return Result.Failed(new BadRequestObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.InvalidLicense, cancellationToken))));

                }

                _busPublish.Send("UpdateSetting", JsonSerializer.Serialize(new { License = setting.License }));

                _cache.Remove("FeatureLicense");

            }


            return Result.SuccessFul();
        }
    }
}
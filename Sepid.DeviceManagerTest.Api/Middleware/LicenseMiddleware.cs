using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NETCore.Encrypt;
using Sepid.DeviceManagerTest.Application.Common.Interfaces;
using Sepid.DeviceManagerTest.Application.Core.Settings.Dto;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Localization;
using Serilog;

namespace Sepid.DeviceManagerTest.Api.Middleware
{
    public class LicenseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILocalization _localization;
        private const string KEY = "BDE6282501A84E0DB9E777D0AE37FED4";
        private const string IV = "2A5658A3A7204E19";
        public static string settingPath = "/setting";
        private readonly IMemoryCache _cache;

        public LicenseMiddleware(RequestDelegate next, ILocalization localization, IMemoryCache cache)
        {
            _next = next;
            _localization = localization;
            _cache = cache;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            string path = httpContext.PurePath().ToLower();

            if (path.Contains(settingPath) == false)
            {
                var context = (IDeviceManagerContext)httpContext.RequestServices.GetService(typeof(IDeviceManagerContext));

                var setting = await context.Settings.FirstOrDefaultAsync();

                if (string.IsNullOrWhiteSpace(setting.License))
                {
                    await httpContext.WriteError(new ApiMessage(_localization.GetMessage(ResponseMessage.LicenseNotFound)), StatusCodes.Status400BadRequest);
                    return;
                }

                if (!_cache.TryGetValue("FeatureLicense", out FeatureLicense featureLicense))
                {
                    var license = "";
                    try
                    {
                        license = EncryptProvider.AESDecrypt(setting.License, KEY, IV);

                        if (string.IsNullOrWhiteSpace(license))
                        {
                            await httpContext.WriteError(new ApiMessage(_localization.GetMessage(ResponseMessage.InvalidLicense)), StatusCodes.Status400BadRequest);
                            return;
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Error(e.Message, e.StackTrace);
                        await httpContext.WriteError(new ApiMessage(_localization.GetMessage(ResponseMessage.InvalidLicense)), StatusCodes.Status400BadRequest);
                        return;
                    }


                    featureLicense = JsonSerializer.Deserialize<FeatureLicense>(license);

                    _cache.Set("FeatureLicense", featureLicense,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(365)));
                }


                if (featureLicense.ExpireDate < DateTime.Now)
                {
                    await httpContext.WriteError(new ApiMessage(_localization.GetMessage(ResponseMessage.LicenseExpired)), StatusCodes.Status400BadRequest);
                    return;
                }

                if (!_cache.TryGetValue("MacAddressNumber", out string systemId))
                {
                    systemId = Utils.GetSystemId();

                    _cache.Set("MacAddressNumber", systemId,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(365)));
                }


                if (systemId != featureLicense.SystemId)
                {
                    await httpContext.WriteError(new ApiMessage(_localization.GetMessage(ResponseMessage.LicenseNotValidForDevice)), StatusCodes.Status400BadRequest);
                    return;

                }



               
            }

            await _next(httpContext);
        }
    }
}
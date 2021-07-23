using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NETCore.Encrypt;
using Sepid.DeviceManagerTest.Application.Core.License.Dto;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Localization;
using Sepid.DeviceManagerTest.Common.Results;
using Serilog;

namespace Sepid.DeviceManagerTest.Application.Core.License.Command.InquiryLicense
{
    public class InquiryLicenseCommandHandler : IRequestHandler<InquiryLicenseCommand, Result<FeatureLicenseDto>>
    {
        private readonly ILocalization _localization;
        private const string KEY = "BDE6282501A84E0DB9E777D0AE37FED4";
        private const string IV = "2A5658A3A7204E19";

        public InquiryLicenseCommandHandler(ILocalization localization)
        {
            _localization = localization;
        }

        public async Task<Result<FeatureLicenseDto>> Handle(InquiryLicenseCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var license = EncryptProvider.AESDecrypt(request.License, KEY, IV);

                if (string.IsNullOrWhiteSpace(license))
                {
                    return Result<FeatureLicenseDto>.Failed(new BadRequestObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.InvalidLicense, cancellationToken))));

                }

                var featureLicense = JsonSerializer.Deserialize<FeatureLicenseDto>(license);

                return Result<FeatureLicenseDto>.SuccessFul(featureLicense);

            }
            catch (Exception e)
            {
                Log.Error(e.Message, e.StackTrace);

            }

            return Result<FeatureLicenseDto>.Failed(new BadRequestObjectResult(new ApiMessage(await _localization.GetMessage(ResponseMessage.InvalidLicense, cancellationToken))));

        }
    }
}
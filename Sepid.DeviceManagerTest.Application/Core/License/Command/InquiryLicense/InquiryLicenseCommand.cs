using MediatR;
using Sepid.DeviceManagerTest.Application.Core.License.Dto;
using Sepid.DeviceManagerTest.Common.Results;

namespace Sepid.DeviceManagerTest.Application.Core.License.Command.InquiryLicense
{
    public class InquiryLicenseCommand : IRequest<Result<FeatureLicenseDto>>
    {
        public string License { get; set; }
    }
}
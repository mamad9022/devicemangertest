using FluentValidation;
using Sepid.DeviceManagerTest.Application.Core.License.Command.InquiryLicense;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Localization;

namespace Sepid.DeviceManagerTest.Application.Common.Validator.License
{
    public class InquiryLicenseCommandValidator : AbstractValidator<InquiryLicenseCommand>
    {
        private readonly ILocalization _localization;

        public InquiryLicenseCommandValidator(ILocalization localization)
        {
            _localization = localization;
            CascadeMode = CascadeMode.Stop;

            RuleFor(dto => dto.License)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.LicenseIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.LicenseIsRequired));
        }
    }
}
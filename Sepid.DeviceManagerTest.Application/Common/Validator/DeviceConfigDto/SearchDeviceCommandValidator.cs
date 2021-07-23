using FluentValidation;
using Sepid.DeviceManagerTest.Application.Core.DeviceConfigs.Command.SearchDeviceCommand;
using Sepid.DeviceManagerTest.Common.Helper;
using Sepid.DeviceManagerTest.Common.Localization;
using System.Net;

namespace Sepid.DeviceManagerTest.Application.Common.Validator.DeviceConfigDto
{
    public class SearchDeviceCommandValidator : AbstractValidator<SearchDeviceCommand>
    {
        private readonly ILocalization _localization;
        public SearchDeviceCommandValidator(ILocalization localization)
        {
            _localization = localization;
            CascadeMode = CascadeMode.Stop;

            RuleFor(dto => dto.Ip)
                .NotEmpty().WithMessage(_localization.GetMessage(ResponseMessage.IpIsRequired))
                .NotNull().WithMessage(_localization.GetMessage(ResponseMessage.IpIsRequired))
                .Must(ValidIpAddress).WithMessage(_localization.GetMessage(ResponseMessage.InvalidIpAddress));

        }

        private bool ValidIpAddress(string ip)
        {
            if (IPAddress.TryParse(ip, out _) == false)
                return false;

            return true;
        }
    }
}